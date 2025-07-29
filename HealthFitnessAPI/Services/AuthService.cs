using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Helpers;
using HealthFitnessAPI.Model.Dtos.Auth;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HealthFitnessAPI.Services;

public interface IAuthService
{
    public Task<LoginResponseDto> Login(LoginDto loginDto);
}

public class AuthService(IUnitOfWork unitOfWork, IConfiguration configuration): IAuthService
{
    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        var user = await unitOfWork.GetDbSet<User>().FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user is null || !Hash.VerifyPassword(loginDto.Password, user.Password)) throw new Exception("Invalid credentials!");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Email, loginDto.Email), new Claim(Claims.Role, user.Role)
            ]),
            Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JwtConfig:ExpiresInDays")),
            Issuer = configuration.GetValue<string>("JwtConfig:Issuer"),
            Audience = configuration.GetValue<string>("JwtConfig:Audience"),
            SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtConfig:Key")!)),
                    SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return new LoginResponseDto()
        {
            AccessToken = tokenHandler.WriteToken(securityToken),
        };
    }
}