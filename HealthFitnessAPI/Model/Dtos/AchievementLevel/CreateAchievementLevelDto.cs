using FluentValidation;

namespace HealthFitnessAPI.Model.Dtos.AchievementLevel;

public class CreateAchievementLevelDto : AbstractAchievementLevelDto
{
}

public class CreateAchievementLevelDtoValidator : AbstractValidator<CreateAchievementLevelDto>
{
    public CreateAchievementLevelDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}