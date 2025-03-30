using HealthFitnessAPI.Constants.Enums;
namespace HealthFitnessAPI.Model.Dtos.User
{
    public class UserResultDto : AbstractUserDto
    {
        public int Id { get; set; }
        public string Gender { get; set; }
    }
}
