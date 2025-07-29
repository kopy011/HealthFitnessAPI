using HealthFitnessAPI.Constants.Enums;
namespace HealthFitnessAPI.Entities
{
    public class User: AbstractEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public Gender Gender { get; set; }
    }
}
