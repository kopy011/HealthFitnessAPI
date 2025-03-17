using HealthFitnessAPI.Constants.Enums;
namespace HealthFitnessAPI.Entities
{
    public class User: AbstractEntity
    {
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public Gender Gender { get; set; }
    }
}
