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
    public Task<LoginResponseDto> AdminLogin(LoginDto loginDto);
    public Task<LoginResponseDto> ValidateRefreshToken(string token);
    public Task RevokeRefreshToken(string token);
}

public class AuthService(IUnitOfWork unitOfWork, IConfiguration configuration) : IAuthService
{
    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        var user = await unitOfWork.GetDbSet<User>().FirstOrDefaultAsync(u => u.Username == loginDto.Username);
        if (user is null || !Hash.VerifyPassword(loginDto.Password!, user.Password!))
            throw new Exception("Invalid credentials!");

        return await GenerateJwtToken(user);
    }

    public async Task<LoginResponseDto> AdminLogin(LoginDto loginDto)
    {
        var user = await unitOfWork.GetDbSet<User>()
            .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.Role == Roles.Admin);
        if (user is null || !Hash.VerifyPassword(loginDto.Password!, user.Password!))
            throw new Exception("Invalid credentials!");

        return await GenerateJwtToken(user);
    }

    public async Task RevokeRefreshToken(string token)
    {
        var refreshToken = await unitOfWork.GetDbSet<RefreshToken>().FirstOrDefaultAsync(rt => rt.Token == token);
        if (refreshToken != null)
        {
            await unitOfWork.GetRepository<RefreshToken>().RemoveAsync(refreshToken);
            await unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<LoginResponseDto> ValidateRefreshToken(string token)
    {
        var refreshToken = await unitOfWork.GetDbSet<RefreshToken>().FirstOrDefaultAsync(rt => rt.Token == token);
        if (refreshToken is null || refreshToken.Expiry < DateTime.UtcNow) throw new Exception("Token not found!");

        await unitOfWork.GetRepository<RefreshToken>().RemoveAsync(refreshToken);
        await unitOfWork.SaveChangesAsync();

        var user = await unitOfWork.GetRepository<User>().GetByIdAsync(refreshToken.UserId);
        if (user == null) throw new Exception("User not found!");

        return await GenerateJwtToken(user);
    }

    private async Task<LoginResponseDto> GenerateJwtToken(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim(ClaimTypes.Role, user.Role!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JwtConfig:ExpiresInMins")),
            Issuer = configuration.GetValue<string>("JwtConfig:Issuer"),
            Audience = configuration.GetValue<string>("JwtConfig:Audience"),
            SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtConfig:Key")!)),
                    SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return new LoginResponseDto
        {
            AccessToken = tokenHandler.WriteToken(securityToken),
            RefreshToken = await GenerateRefreshToken(user.Id)
        };
    }

    private async Task<string> GenerateRefreshToken(int userId)
    {
        var expiryDate = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JwtConfig:RefreshTokenExpiresInMins"));
        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = userId,
            Expiry = expiryDate
        };

        await unitOfWork.GetRepository<RefreshToken>().CreateAsync(refreshToken);
        await unitOfWork.SaveChangesAsync();

        return refreshToken.Token;
    }
}