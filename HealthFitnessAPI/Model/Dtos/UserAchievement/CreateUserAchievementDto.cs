using FluentValidation;

namespace HealthFitnessAPI.Model.Dtos.UserAchievement;

public class CreateUserAchievementDto : AbstractUserAchievementDto
{
}

public class CreateUserAchievementDtoValidator : AbstractValidator<CreateUserAchievementDto>
{
    public CreateUserAchievementDtoValidator()
    {
        RuleFor(u => u.AchievementLevelId).NotNull();
        RuleFor(u => u.UserId).NotNull();
        RuleFor(u => u.AchievementId).NotNull();
    }
}