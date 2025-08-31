using FluentValidation;

namespace HealthFitnessAPI.Model.Dtos.AchievementLevelThreshold;

public class CreateAchievementLevelThresholdDto : AbstractAchievementLevelThresholdDto
{
    public string LogoBase64 { get; set; }
}

public class CreateAchievementLevelThresholdDtoValidator : AbstractValidator<CreateAchievementLevelThresholdDto>
{
    public CreateAchievementLevelThresholdDtoValidator()
    {
        RuleFor(a => a.AchievementLevelId).NotEmpty();
        RuleFor(a => a.FemaleThreshold).NotEmpty();
        RuleFor(a => a.MaleThreshold).NotEmpty();
        RuleFor(a => a.LogoBase64).NotEmpty();
    }
}