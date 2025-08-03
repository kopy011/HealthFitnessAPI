namespace HealthFitnessAPI.Model.Dtos.Auth;

public class LoginResponseDto
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}