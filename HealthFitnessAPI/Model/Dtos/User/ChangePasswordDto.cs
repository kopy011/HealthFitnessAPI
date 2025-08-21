using FluentValidation;

namespace HealthFitnessAPI.Model.Dtos.User;

public class ChangePasswordDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? NewPassword { get; set; }
}

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(u => u.Username).NotEmpty();
        RuleFor(cp => cp.Password).NotEmpty();
        RuleFor(cp => cp.NewPassword).NotEmpty();
    }
}

public class AdminChangePasswordDto
{
    public int? Id { get; set; }
    public string? Password { get; set; }
}

public class AdminChangePasswordDtoValidator : AbstractValidator<AdminChangePasswordDto>
{
    public AdminChangePasswordDtoValidator()
    {
        RuleFor(u => u.Id).NotEmpty();
        RuleFor(cp => cp.Password).NotEmpty();
    }
}