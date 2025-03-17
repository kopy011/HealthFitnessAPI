using HealthFitnessAPI.Constants.Enums;
namespace HealthFitnessAPI.Model.Dtos.User
{
    public class AbstractUserDto
    {
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public Gender Gender { get; set; }
    }

}
