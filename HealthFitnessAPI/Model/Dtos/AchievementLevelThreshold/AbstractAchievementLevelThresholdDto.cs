namespace HealthFitnessAPI.Model.Dtos.AchievementLevelThreshold;

public class AbstractAchievementLevelThresholdDto
{
    public int MaleThreshold { get; set; }
    public int FemaleThreshold { get; set; }
    public string ThresholdType { get; set; }
    public int AchievementLevelId { get; set; }
    public int Order { get; set; }
}