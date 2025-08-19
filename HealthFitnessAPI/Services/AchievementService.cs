using HealthFitnessAPI.Entities;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HealthFitnessAPI.Services;

public interface IAchievementService : IAbstractService<Achievement>
{
    Task<List<Achievement>> GetAllWithThresholds();
}

public class AchievementService(IUnitOfWork unitOfWork)
    : AbstractService<Achievement>(unitOfWork), IAchievementService
{
    public override async Task<Achievement> Update(Achievement entity)
    {
        await unitOfWork.GetRepository<AchievementLevelThreshold>().RemoveRangeAsync(entity.AchievementLevelThresholds);
        await unitOfWork.SaveChangesAsync();
        return await base.Update(entity);
    }

    public async Task<List<Achievement>> GetAllWithThresholds()
    {
        return await unitOfWork.GetRepository<Achievement>().GetAllAsQueryable()
            .Include(a => a.AchievementLevelThresholds).ToListAsync();
    }
}