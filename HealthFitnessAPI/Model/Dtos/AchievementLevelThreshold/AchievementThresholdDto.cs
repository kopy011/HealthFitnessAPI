using FluentValidation;
using HealthFitnessAPI.Model.Dtos.AchievementLevel;

namespace HealthFitnessAPI.Model.Dtos.AchievementLevelThreshold;

public class AchievementLevelThresholdDto
{
    public int MaleThreshold { get; set; }
    public int FemaleThreshold { get; set; }
    public int AchievementLevelId { get; set; }
}

public class AchievementLevelThresholdResultDto
{
    public int MaleThreshold { get; set; }
    public int FemaleThreshold { get; set; }
    public AchievementLevelResultDto AchievementLevel { get; set; }
}

public class AchievementLevelThresholdDtoValidator : AbstractValidator<AchievementLevelThresholdDto>
{
    public AchievementLevelThresholdDtoValidator()
    {
        RuleFor(a => a.AchievementLevelId).NotEmpty();
        RuleFor(a => a.MaleThreshold).NotEmpty();
        RuleFor(a => a.FemaleThreshold).NotEmpty();
    }
}