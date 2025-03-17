using HealthFitnessAPI.Entities;
using HealthFitnessAPI.UnitOfWork;
namespace HealthFitnessAPI.Services
{
    public interface IAchievementService : IAbstractService<Achievement> { }

    public class AchievementService(IUnitOfWork unitOfWork) : AbstractService<Achievement>(unitOfWork), IAchievementService { }
}
