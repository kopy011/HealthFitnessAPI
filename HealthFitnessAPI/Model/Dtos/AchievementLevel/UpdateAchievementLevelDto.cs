using FluentValidation;

namespace HealthFitnessAPI.Model.Dtos.AchievementLevel;

public class UpdateAchievementLevelDto : AbstractAchievementLevelDto
{
    public int Id { get; set; }
}

public class UpdateAchievementLevelDtoValidator : AbstractValidator<UpdateAchievementLevelDto>
{
    public UpdateAchievementLevelDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
    }
}