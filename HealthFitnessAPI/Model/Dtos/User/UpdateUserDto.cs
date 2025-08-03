using FluentValidation;

namespace HealthFitnessAPI.Model.Dtos.User;

public class UpdateUserDto : AbstractUserDto
{
    public int Id { get; set; }
}

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(u => u.Id).NotEmpty();
        RuleFor(u => u.Username).NotEmpty();
        RuleFor(u => u.Email).EmailAddress().When(u => !string.IsNullOrEmpty(u.Email));
        RuleFor(u => u.FullName).NotEmpty();
        RuleFor(u => u.DisplayName).NotEmpty();
        RuleFor(u => u.Gender).NotEmpty();
    }
}