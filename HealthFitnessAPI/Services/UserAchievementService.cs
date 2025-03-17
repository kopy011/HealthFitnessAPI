using HealthFitnessAPI.Entities;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;
namespace HealthFitnessAPI.Services
{
    public interface IUserAchievementService : IAbstractService<UserAchievement>
    {
        Task<List<UserAchievement>> GetAllWithInclude();
    }

    public class UserAchievementService(IUnitOfWork unitOfWork) : AbstractService<UserAchievement>(unitOfWork), IUserAchievementService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<UserAchievement>> GetAllWithInclude()
        {
            return await _unitOfWork.GetDbContext().Set<UserAchievement>().Include(ua => ua.User).Include(ua => ua.Achievement).AsNoTracking().ToListAsync();
        }
    }
}
