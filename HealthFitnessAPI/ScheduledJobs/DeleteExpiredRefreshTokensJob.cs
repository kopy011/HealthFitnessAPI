using HealthFitnessAPI.Entities;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace HealthFitnessAPI.ScheduledJobs;

public class DeleteExpiredRefreshTokensJob(IUnitOfWork unitOfWork) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Deleting expired refresh tokens!");
        
        var expiredRefreshTokens = unitOfWork.GetDbSet<RefreshToken>().Where(rt => rt.Expiry < DateTime.UtcNow).ToList();
        await unitOfWork.GetRepository<RefreshToken>().RemoveRangeAsync(expiredRefreshTokens);
        await unitOfWork.SaveChangesAsync();
        
        Console.WriteLine("Deleted all expired refresh tokens!");
    }
}