namespace HealthFitnessAPI.Entities;

public class Achievement : AbstractEntity
{
    public string Category { get; set; }
    public string? Description { get; set; }
    public List<AchievementLevelThreshold> AchievementLevelThresholds { get; set; } = [];
}