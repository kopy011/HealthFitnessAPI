using FluentValidation;

namespace HealthFitnessAPI.Model.Dtos.Auth
{
    public class RefreshDto
    {
        public string? Token { get; set; }
    }

    public class RefreshDtoValidator : AbstractValidator<RefreshDto>
    {
        public RefreshDtoValidator()
        {
            RuleFor(r => r.Token).NotEmpty();
        }
    }
}