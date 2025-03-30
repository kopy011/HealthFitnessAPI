using FluentValidation;
namespace HealthFitnessAPI.Model.Dtos.User
{
    public class CreateUserDto : AbstractUserDto
    {
        public string Gender { get; set; }
    }

    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(u => u.FullName).NotEmpty();
            RuleFor(u => u.Nickname).NotEmpty();
            RuleFor(u => u.Gender).NotEmpty();
        }
    }
}
