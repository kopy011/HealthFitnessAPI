using FluentValidation;
using HealthFitnessAPI.Model.Dtos.AchievementLevelThreshold;

namespace HealthFitnessAPI.Model.Dtos.Achievement;

public class CreateAchievementDto : AbstractAchievementDto
{
    public List<AchievementLevelThresholdDto> AchievementLevelThresholds { get; set; }
}

public class CreateAchievementDtoValidator : AbstractValidator<CreateAchievementDto>
{
    public CreateAchievementDtoValidator()
    {
        RuleFor(a => a.Description).NotEmpty();
        RuleFor(a => a.AchievementLevelThresholds).NotEmpty();
        RuleForEach(a => a.AchievementLevelThresholds).SetValidator(new AchievementLevelThresholdDtoValidator());
    }
}