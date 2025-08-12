using HealthFitnessAPI.Constants.Enums;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Helpers;
using HealthFitnessAPI.Model.Dtos.User;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HealthFitnessAPI.Services;

public interface IUserService : IAbstractService<User>
{
    Task<User> ChangePassword(ChangePasswordDto changePasswordDto);
    Task<List<User>> GetFriends(int userId);
    Task<List<Friendship>> GetSentPendingFriendRequests(int userId);
    Task<List<Friendship>> GetReceivedPendingFriendRequests(int userId);
    Task AddFriend(int userId, int friendId);
    Task RemoveFriend(int userId, int friendId);
    Task AcceptFriendRequest(int userId, int friendId);
    Task DeclineFriendRequest(int userId, int friendId);
}

public class UserService(IUnitOfWork unitOfWork, IAuthService authService)
    : AbstractService<User>(unitOfWork), IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<User> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var user = await _unitOfWork.GetDbSet<User>()
            .FirstOrDefaultAsync(u => u.Username == changePasswordDto.Username);
        if (user == null || !Hash.VerifyPassword(changePasswordDto.Password!, user.Password!))
            throw new Exception("Invalid username or password!");

        user.Password = Hash.HashPassword(changePasswordDto.NewPassword!);
        await _unitOfWork.SaveChangesAsync();

        var refreshToken = await unitOfWork.GetDbSet<RefreshToken>().FirstOrDefaultAsync(rt => rt.UserId == user.Id);
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

        await unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveFriend(int userId, int friendId)
    {
        var user = await GetByIdWithInclude(userId, true);

        user.FriendsSent
            .RemoveAll(f => f.UserId == userId && f.FriendId == friendId && f.Status == FriendshipStatus.Accepted);
        user.FriendsRecieved
            .RemoveAll(f => f.FriendId == userId && f.UserId == friendId && f.Status == FriendshipStatus.Accepted);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task AcceptFriendRequest(int userId, int friendId)
    {
        var user = await GetByIdWithInclude(userId, true);
        var friendShip = user.FriendsRecieved.FirstOrDefault(f => f.UserId == friendId);
        if (friendShip == null) throw new Exception("Friendship not found!");
        friendShip.Status = FriendshipStatus.Accepted;
        friendShip.LastUpdated = DateTime.UtcNow;
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeclineFriendRequest(int userId, int friendId)
    {
        var user = await GetByIdWithInclude(userId, true);

        user.FriendsRecieved
            .RemoveAll(f => f.UserId == friendId && f.FriendId == userId && f.Status == FriendshipStatus.Pending);

        await unitOfWork.SaveChangesAsync();
    }

    private async Task<User> GetByIdWithInclude(int userId, bool track = false)
    {
        var user = await unitOfWork.GetRepository<User>().GetAllAsQueryable(track)
            .Include(u => u.FriendsSent)
            .ThenInclude(f => f.Friend)
            .Include(u => u.FriendsRecieved)
            .ThenInclude(f => f.User)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user ?? throw new Exception("Entity not found!");
    }
}