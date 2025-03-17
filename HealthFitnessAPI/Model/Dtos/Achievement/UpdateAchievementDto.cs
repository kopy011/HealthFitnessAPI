using FluentValidation;
namespace HealthFitnessAPI.Model.Dtos.Achievement
{
    public class UpdateAchievementDto : AbstractAchievementDto
    {
        public int Id { get; set; }
    }

    public class UpdateAchievementDtoValidator : AbstractValidator<UpdateAchievementDto>
    {
        public UpdateAchievementDtoValidator()
        {
            RuleFor(a => a.Id).NotEmpty();
            RuleFor(a => a.Name).NotEmpty();
            RuleFor(a => a.Description).NotEmpty();
        }
    }
}
