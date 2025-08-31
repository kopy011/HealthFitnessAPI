namespace HealthFitnessAPI.Model.Dtos.AchievementLevelThreshold;

public class AchievementLevelThresholdResultDto : AbstractAchievementLevelThresholdDto
{
    public int Id { get; set; }
    public string LogoPath { get; set; }
}

public class AchievementLevelThresholdResultWithImageDto : AchievementLevelThresholdResultDto
{
    public string LogoBase64 { get; set; }
}