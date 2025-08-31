using System.ComponentModel.DataAnnotations;

namespace HealthFitnessAPI.Entities;

public class AchievementLevel : AbstractEntity
{
    [Required] public string Name { get; set; }
}