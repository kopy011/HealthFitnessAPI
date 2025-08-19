using System.ComponentModel.DataAnnotations;

namespace HealthFitnessAPI.Entities;

public class AchievementLevelThreshold : AbstractEntity
{
    [Required] public int FemaleThreshold { get; set; }
    [Required] public int MaleThreshold { get; set; }

    [Required] public int AchievementLevelId { get; set; }

    public AchievementLevel? AchievementLevel { get; set; }
}