using FluentValidation;
namespace HealthFitnessAPI.Model.Dtos.User
{
    public class UpdateUserDto : AbstractUserDto
    {
        public int Id { get; set; }
    }

    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(u => u.Id).NotEmpty();
            RuleFor(u => u.FullName).NotEmpty();
            RuleFor(u => u.Nickname).NotEmpty();
            RuleFor(u => u.Gender).NotEmpty();
        }
    }
}
