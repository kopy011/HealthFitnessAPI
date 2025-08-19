using FluentValidation;
using HealthFitnessAPI.Model.Dtos.AchievementLevelThreshold;

namespace HealthFitnessAPI.Model.Dtos.Achievement;

public class UpdateAchievementDto : AbstractAchievementDto
{
    public int Id { get; set; }
    public List<AchievementLevelThresholdDto> AchievementLevelThresholds { get; set; }
}

public class UpdateAchievementDtoValidator : AbstractValidator<UpdateAchievementDto>
{
    public UpdateAchievementDtoValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
        RuleFor(a => a.Description).NotEmpty();
        RuleFor(a => a.AchievementLevelThresholds).NotEmpty();
        RuleForEach(a => a.AchievementLevelThresholds).SetValidator(new AchievementLevelThresholdDtoValidator());
    }
}