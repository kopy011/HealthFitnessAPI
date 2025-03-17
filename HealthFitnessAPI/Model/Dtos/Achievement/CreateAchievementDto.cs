using FluentValidation;
namespace HealthFitnessAPI.Model.Dtos.Achievement
{
    public class CreateAchievementDto : AbstractAchievementDto { }

    public class CreateAchievementDtoValidator : AbstractValidator<CreateAchievementDto>
    {
        public CreateAchievementDtoValidator()
        {
            RuleFor(a => a.Name).NotEmpty();
            RuleFor(a => a.Description).NotEmpty();
        }
    }
}
