using FluentValidation;

namespace HealthFitnessAPI.Model.Dtos.AchievementLevelThreshold;

public class UpdateAchievementLevelThresholdDto : AbstractAchievementLevelThresholdDto
{
    public string LogoBase64 { get; set; }
}

public class UpdateAchievementLevelThresholdDtoValidator : AbstractValidator<UpdateAchievementLevelThresholdDto>
{
    public UpdateAchievementLevelThresholdDtoValidator()
    {
        RuleFor(a => a.AchievementLevelId).NotEmpty();
        RuleFor(a => a.FemaleThreshold).NotEmpty();
        RuleFor(a => a.MaleThreshold).NotEmpty();
        RuleFor(a => a.LogoBase64).NotEmpty();
    }
}