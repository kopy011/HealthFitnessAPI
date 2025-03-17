using HealthFitnessAPI.Entities;
using HealthFitnessAPI.UnitOfWork;
namespace HealthFitnessAPI.Services
{
    public interface IUserService : IAbstractService<User> { }

    public class UserService(IUnitOfWork unitOfWork) : AbstractService<User>(unitOfWork), IUserService { }
}
