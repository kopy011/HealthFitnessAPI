using HealthFitnessAPI.Model.Dtos.AchievementLevelThreshold;

namespace HealthFitnessAPI.Model.Dtos.Achievement;

public class AchievementResultDto : AbstractAchievementDto
{
    public int Id { get; set; }
    public List<AchievementLevelThresholdResultWithImageDto> AchievementLevelThresholds { get; set; }
}

public class AchievementPathOnlyResultDto : AbstractAchievementDto
{
    public int Id { get; set; }
    public List<AchievementLevelThresholdResultDto> AchievementLevelThresholds { get; set; }
}