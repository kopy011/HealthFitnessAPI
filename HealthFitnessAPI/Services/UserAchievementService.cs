using HealthFitnessAPI.Entities;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HealthFitnessAPI.Services;

public interface IUserAchievementService : IAbstractService<UserAchievement>
{
    Task<List<UserAchievement>> GetAllWithInclude();
    Task<List<UserAchievement>> GetAllByUserId(int userId);
    Task<IEnumerable<UserAchievement>> GetAllByUserIds(List<int> userIds);
}

public class UserAchievementService(IUnitOfWork unitOfWork)
    : AbstractService<UserAchievement>(unitOfWork), IUserAchievementService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<UserAchievement>> GetAllWithInclude()
    {
        return await _unitOfWork.GetDbContext().Set<UserAchievement>().Include(ua => ua.User)
            .Include(ua => ua.Achievement)
            .Include(ua => ua.AchievementLevel).AsNoTracking().ToListAsync();
    }

    public async Task<List<UserAchievement>> GetAllByUserId(int userId)
    {
        return await _unitOfWork.GetRepository<UserAchievement>().GetAllAsQueryable().Where(u => u.UserId == userId)
            .Include(u => u.User)
            .Include(u => u.Achievement)
            .Include(u => u.AchievementLevel)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserAchievement>> GetAllByUserIds(List<int> userIds)
    {
        return await _unitOfWork.GetRepository<UserAchievement>().GetAllAsQueryable()
            .Where(u => userIds.Contains(u.UserId))
            .Include(u => u.User)
            .Include(u => u.Achievement).ToListAsync();
    }
}