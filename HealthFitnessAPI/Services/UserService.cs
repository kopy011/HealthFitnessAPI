using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Constants.Enums;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Helpers;
using HealthFitnessAPI.Model;
using HealthFitnessAPI.Model.Dtos.User;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HealthFitnessAPI.Services;

public interface IUserService : IAbstractService<User>
{
    Task<User> ChangePassword(ChangePasswordDto changePasswordDto);
    Task<User> AdminChangePassword(AdminChangePasswordDto adminChangePassword);
    Task<List<User>> GetFriends(int userId);
    Task<List<Friendship>> GetSentPendingFriendRequests(int userId);
    Task<List<Friendship>> GetReceivedPendingFriendRequests(int userId);
    Task AddFriend(int userId, int friendId);
    Task RemoveFriend(int userId, int friendId);
    Task AcceptFriendRequest(int userId, int friendId);
    Task DeclineFriendRequest(int userId, int friendId);

    Task<List<UserAchievement>> GetFeed(int userId, PaginationDto pagination, FeedOrderBy orderBy,
        string? queryString = null);
}

public class UserService(
    IUnitOfWork unitOfWork,
    IAuthService authService,
    IUserAchievementService userAchievementService)
    : AbstractService<User>(unitOfWork), IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public override async Task<List<User>> GetAll(bool track = false)
    {
        return await _unitOfWork.GetRepository<User>().GetAllAsQueryable(track).Where(u => u.Role == Roles.User)
            .ToListAsync();
    }

    public async Task<User> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var user = await _unitOfWork.GetDbSet<User>()
            .FirstOrDefaultAsync(u => u.Username == changePasswordDto.Username);
        if (user == null || !Hash.VerifyPassword(changePasswordDto.Password!, user.Password!))
            throw new Exception("Invalid username or password!");

        user.Password = Hash.HashPassword(changePasswordDto.NewPassword!);
        await _unitOfWork.SaveChangesAsync();

        var refreshToken = await _unitOfWork.GetDbSet<RefreshToken>().FirstOrDefaultAsync(rt => rt.UserId == user.Id);
        if (refreshToken != null) await authService.RevokeRefreshToken(refreshToken.Token!);

        return user;
    }

    public async Task<User> AdminChangePassword(AdminChangePasswordDto adminChangePassword)
    {
        var user = await _unitOfWork.GetDbSet<User>()
            .FirstOrDefaultAsync(u => u.Id == adminChangePassword.Id);
        if (user == null)
            throw new Exception("Invalid username!");

        user.Password = Hash.HashPassword(adminChangePassword.Password!);
        await _unitOfWork.SaveChangesAsync();

        var refreshToken = await _unitOfWork.GetDbSet<RefreshToken>().FirstOrDefaultAsync(rt => rt.UserId == user.Id);
        if (refreshToken != null) await authService.RevokeRefreshToken(refreshToken.Token!);

        return user;
    }

    public async Task<List<User>> GetFriends(int userId)
    {
        var user = await GetByIdWithInclude(userId);
        return user.FriendsSent.Where(f => f.Status == FriendshipStatus.Accepted).Select(f => f.Friend)
            .Union(user.FriendsRecieved.Where(f => f.Status == FriendshipStatus.Accepted).Select(f => f.User))
            .DistinctBy(u => u.Id).ToList();
    }

    public override async Task<User> Update(User entity)
    {
        var userInDb = await GetById(entity.Id);
        entity.Password = userInDb.Password;
        entity.Role = userInDb.Role;
        return await base.Update(entity);
    }

    public async Task<List<Friendship>> GetSentPendingFriendRequests(int userId)
    {
        var user = await GetByIdWithInclude(userId);
        return user.FriendsSent.Where(f => f.Status == FriendshipStatus.Pending).ToList();
    }

    public async Task<List<Friendship>> GetReceivedPendingFriendRequests(int userId)
    {
        var user = await GetByIdWithInclude(userId);
        return user.FriendsRecieved.Where(f => f.Status == FriendshipStatus.Pending).ToList();
    }

    public async Task AddFriend(int userId, int friendId)
    {
        if (userId == friendId) throw new Exception("You cannot add yourself as a friend!");

        var user = await GetByIdWithInclude(userId, true);
        var friend = await GetByIdWithInclude(friendId, true);

        if (user.FriendsSent.Any(f => f.FriendId == friendId) || user.FriendsRecieved.Any(f => f.UserId == friendId))
            throw new Exception("Friend request already sent!");

        user.FriendsSent.Add(new Friendship
        {
            UserId = user.Id,
            FriendId = friend.Id,
            Status = FriendshipStatus.Pending,
            LastUpdated = DateTime.UtcNow
        });

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveFriend(int userId, int friendId)
    {
        var user = await GetByIdWithInclude(userId, true);

        user.FriendsSent
            .RemoveAll(f => f.UserId == userId && f.FriendId == friendId && f.Status == FriendshipStatus.Accepted);
        user.FriendsRecieved
            .RemoveAll(f => f.FriendId == userId && f.UserId == friendId && f.Status == FriendshipStatus.Accepted);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AcceptFriendRequest(int userId, int friendId)
    {
        var user = await GetByIdWithInclude(userId, true);
        var friendShip = user.FriendsRecieved.FirstOrDefault(f => f.UserId == friendId);
        if (friendShip == null) throw new Exception("Friendship not found!");
        friendShip.Status = FriendshipStatus.Accepted;
        friendShip.LastUpdated = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeclineFriendRequest(int userId, int friendId)
    {
        var user = await GetByIdWithInclude(userId, true);

        user.FriendsRecieved
            .RemoveAll(f => f.UserId == friendId && f.FriendId == userId && f.Status == FriendshipStatus.Pending);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<UserAchievement>> GetFeed(int userId, PaginationDto pagination, FeedOrderBy orderBy,
        string? queryString = null)
    {
        var friends = await GetFriends(userId);
        var userAchievements = (await userAchievementService.GetAllByUserIds(friends.Select(f => f.Id).ToList()))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryString))
        {
            var lowerCaseQuery = queryString.ToLower();
            userAchievements = userAchievements.Where(u =>
                u.User!.Username!.Contains(lowerCaseQuery, StringComparison.CurrentCultureIgnoreCase) ||
                u.Achievement!.Category!.Contains(lowerCaseQuery, StringComparison.CurrentCultureIgnoreCase));
        }

        userAchievements = orderBy switch
        {
            FeedOrderBy.Trending or FeedOrderBy.DateDescending => //TODO likeok implementálása
                userAchievements.OrderByDescending(u => u.CreatedAt),
            FeedOrderBy.DateAscending => userAchievements.OrderBy(u => u.CreatedAt),
            FeedOrderBy.AToZ => userAchievements.OrderBy(u => u.User!.Username),
            FeedOrderBy.ZToA => userAchievements.OrderByDescending(u => u.User!.Username),
            _ => throw new ArgumentOutOfRangeException()
        };

        return userAchievements.Skip((pagination.CurrentPage - 1) * pagination.PageSize).Take(pagination.PageSize)
            .ToList();
    }

    private async Task<User> GetByIdWithInclude(int userId, bool track = false)
    {
        var user = await _unitOfWork.GetRepository<User>().GetAllAsQueryable(track)
            .Include(u => u.FriendsSent)
            .ThenInclude(f => f.Friend)
            .Include(u => u.FriendsRecieved)
            .ThenInclude(f => f.User)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user ?? throw new Exception("Entity not found!");
    }
}