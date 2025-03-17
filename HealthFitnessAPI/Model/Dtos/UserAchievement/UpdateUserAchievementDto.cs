using FluentValidation;
namespace HealthFitnessAPI.Model.Dtos.UserAchievement
{
    public class UpdateUserAchievementDto : AbstractUserAchievementDto
    {
        public int Id { get; set; }
    }

    public class UpdateUserAchievementDtoValidator : AbstractValidator<UpdateUserAchievementDto>
    {
        public UpdateUserAchievementDtoValidator()
        {
            RuleFor(u => u.Id).NotEmpty();
            RuleFor(u => u.AchievementLevel).NotNull();
            RuleFor(u => u.UserId).NotNull();
            RuleFor(u => u.AchievementId).NotNull();
        }
    }
}
