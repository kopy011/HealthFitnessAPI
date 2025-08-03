using FluentValidation;

namespace HealthFitnessAPI.Model.Dtos.User;

public class CreateUserDto : AbstractUserDto
{
    public string? Password { get; set; }
}

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(u => u.Username).NotEmpty();
        RuleFor(u => u.Password).NotEmpty();
        RuleFor(u => u.Email).EmailAddress().When(u => !string.IsNullOrEmpty(u.Email));
        RuleFor(u => u.FullName).NotEmpty();
        RuleFor(u => u.DisplayName).NotEmpty();
        RuleFor(u => u.Gender).NotEmpty();
    }
}