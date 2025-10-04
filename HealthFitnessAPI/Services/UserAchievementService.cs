using HealthFitnessAPI.Entities;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HealthFitnessAPI.Services;

public interface IUserAchievementService : IAbstractService<UserAchievement>
{
    Task<UserAchievement> GetByIdWithInclude(int id, bool track = false);
    Task<List<UserAchievement>> GetAllWithInclude();
    Task<List<UserAchievement>> GetAllByUserId(int userId);
    Task<IEnumerable<UserAchievement>> GetAllByUserIds(List<int> userIds);
}

public class UserAchievementService(IUnitOfWork unitOfWork)
    : AbstractService<UserAchievement>(unitOfWork), IUserAchievementService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public override async Task<UserAchievement> Create(UserAchievement entity)
    {
        var achievement = await unitOfWork.GetRepository<Achievement>().GetAllAsQueryable()
            .Include(a => a.AchievementLevelThresholds).FirstOrDefaultAsync(a => a.Id == entity.AchievementId);
        if (achievement == null ||
            achievement.AchievementLevelThresholds.All(alt => alt.AchievementLevelId != entity.AchievementLevelId))
            throw new Exception("Achievement cannot be completed on the given level!");

        return await base.Create(entity);
    }

    public async Task<UserAchievement> GetByIdWithInclude(int id, bool track = false)
    {
        if (track)
            return await _unitOfWork.GetDbContext().Set<UserAchievement>()
                       .Include(ua => ua.User)
                       .Include(ua => ua.Achievement)
                       .Include(ua => ua.AchievementLevel)
                       .Include(ua => ua.Likes)
                       .AsTracking().FirstOrDefaultAsync(ua => ua.Id == id) ??
                   throw new Exception("User Achievement not found!");

        return await _unitOfWork.GetDbContext().Set<UserAchievement>()
                   .Include(ua => ua.User)
                   .Include(ua => ua.Achievement)
                   .Include(ua => ua.AchievementLevel)
                   .Include(ua => ua.Likes)
                   .AsNoTracking().FirstOrDefaultAsync(ua => ua.Id == id) ??
               throw new Exception("User Achievement not found!");
    }

    public async Task<List<UserAchievement>> GetAllWithInclude()
    {
        return await _unitOfWork.GetDbContext().Set<UserAchievement>()
            .Include(ua => ua.User)
            .Include(ua => ua.Achievement)
            .Include(ua => ua.Likes)
            .Include(ua => ua.AchievementLevel).AsNoTracking().ToListAsync();
    }

    public async Task<List<UserAchievement>> GetAllByUserId(int userId)
    {
        return await _unitOfWork.GetRepository<UserAchievement>().GetAllAsQueryable().Where(u => u.UserId == userId)
            .Include(u => u.User)
            .Include(u => u.Achievement)
            .ThenInclude(u => u.AchievementLevelThresholds)
            .Include(ua => ua.Likes)
            .Include(u => u.AchievementLevel)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserAchievement>> GetAllByUserIds(List<int> userIds)
    {
        return await _unitOfWork.GetRepository<UserAchievement>().GetAllAsQueryable()
            .Where(u => userIds.Contains(u.UserId))
            .Include(u => u.User)
            .Include(u => u.Achievement)
            .ThenInclude(a => a.AchievementLevelThresholds)
            .Include(u => u.AchievementLevel)
            .Include(ua => ua.Likes)
            .ToListAsync();
    }
}