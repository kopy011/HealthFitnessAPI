using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Helpers;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HealthFitnessAPI.Services.Init;

public interface IUserInitService
{
    public Task InitUsers();
}

public class UserInitService(IUnitOfWork unitOfWork, IConfiguration configuration) : IUserInitService
{
    public async Task InitUsers()
    {
        var adminUser = await unitOfWork.GetDbSet<User>()
            .FirstOrDefaultAsync(user => user.Email == configuration["DefaultAdminUser:Email"]);

        if (adminUser != null) return;

        configuration.GetValue<string>("DefaultAdminUser:Password");

        await unitOfWork.GetRepository<User>().CreateAsync(new User
        {
            Username = configuration["DefaultAdminUser:Username"]!,
            Email = configuration["DefaultAdminUser:Email"]!,
            Password = Hash.HashPassword(configuration["DefaultAdminUser:Password"]!),
            Role = Roles.Admin,
            FullName = "Administrator",
            DisplayName = "Admin"
        });

        await unitOfWork.SaveChangesAsync();
    }
}