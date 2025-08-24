using FluentValidation;

namespace HealthFitnessAPI.Model.Dtos.User;

public class UpdateUserProfileDto
{
    public string Username { get; set; }
    public string? Email { get; set; }
    public string FullName { get; set; }
    public string DisplayName { get; set; }
    public string Gender { get; set; }
}

public class UpdateUserProfileDtoValidator : AbstractValidator<UpdateUserProfileDto>
{
    public UpdateUserProfileDtoValidator()
    {
        RuleFor(u => u.Username).NotEmpty();
        RuleFor(u => u.Email).EmailAddress().When(u => !string.IsNullOrEmpty(u.Email));
        RuleFor(u => u.FullName).NotEmpty();
        RuleFor(u => u.DisplayName).NotEmpty();
        RuleFor(u => u.Gender).NotEmpty();
    }
}