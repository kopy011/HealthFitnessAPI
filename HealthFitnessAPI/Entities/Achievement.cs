using HealthFitnessAPI.Constants.Enums;
namespace HealthFitnessAPI.Entities
{
    public class Achievement : AbstractEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public AchievementType AchievementType { get; set; }
    }
}
