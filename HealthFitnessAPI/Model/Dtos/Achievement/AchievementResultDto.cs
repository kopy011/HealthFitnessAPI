using HealthFitnessAPI.Model.Dtos.AchievementLevelThreshold;

namespace HealthFitnessAPI.Model.Dtos.Achievement;

public class AchievementResultDto : AbstractAchievementDto
{
    public int Id { get; set; }
    public List<AchievementLevelThresholdResultDto> AchievementLevelThresholds { get; set; }
}