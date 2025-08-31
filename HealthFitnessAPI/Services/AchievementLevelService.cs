using HealthFitnessAPI.Entities;
using HealthFitnessAPI.UnitOfWork;

namespace HealthFitnessAPI.Services;

public interface IAchievementLevelService : IAbstractService<AchievementLevel>
{
}

public class AchievementLevelService(IUnitOfWork unitOfWork)
    : AbstractService<AchievementLevel>(unitOfWork), IAchievementLevelService
{
}