using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Helpers;
using HealthFitnessAPI.Model.Dtos.User;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HealthFitnessAPI.Services;

public interface IUserService : IAbstractService<User>
{
    Task<User> ChangePassword(ChangePasswordDto changePasswordDto);
}

public class UserService(IUnitOfWork unitOfWork, IAuthService authService)
    : AbstractService<User>(unitOfWork), IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<User> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var user = await _unitOfWork.GetDbSet<User>()
            .FirstOrDefaultAsync(u => u.Username == changePasswordDto.Username);
        if (user == null || !Hash.VerifyPassword(changePasswordDto.Password!, user.Password!))
            throw new Exception("Invalid username or password!");

        user.Password = Hash.HashPassword(changePasswordDto.NewPassword!);
        await _unitOfWork.SaveChangesAsync();

        var refreshToken = await unitOfWork.GetDbSet<RefreshToken>().FirstOrDefaultAsync(rt => rt.UserId == user.Id);
        if (refreshToken != null) await authService.RevokeRefreshToken(refreshToken.Token!);

        return user;
    }
}