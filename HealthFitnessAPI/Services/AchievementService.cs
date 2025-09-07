using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.Achievement;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HealthFitnessAPI.Services;

public interface IAchievementService : IAbstractService<Achievement>
{
    Task<Achievement> CreateWithUpload(CreateAchievementDto dto);
    Task<Achievement> UpdateWithUpload(UpdateAchievementDto dto);
    Task<AchievementResultDto> GetByIdWithThresholds(int id);
}

public class AchievementService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
    : AbstractService<Achievement>(unitOfWork), IAchievementService
{
    public async Task<Achievement> CreateWithUpload(CreateAchievementDto dto)
    {
        var achievement = mapper.Map<Achievement>(dto);

        foreach (var alt in achievement.AchievementLevelThresholds)
        {
            var imagePath = await fileService.SaveBase64PngAsync(
                dto.AchievementLevelThresholds.FirstOrDefault(lt => lt.AchievementLevelId == alt.AchievementLevelId)!
                    .LogoBase64, $"{achievement.Category}_{alt.AchievementLevelId}.png");
            alt.LogoPath = imagePath;
        }

        return await base.Create(achievement);
    }

    public async Task<Achievement> UpdateWithUpload(UpdateAchievementDto dto)
    {
        var achievementInDb = await unitOfWork.GetRepository<Achievement>().GetAllAsQueryable()
            .Include(a => a.AchievementLevelThresholds).FirstOrDefaultAsync(a => a.Id == dto.Id);
        if (achievementInDb == null) throw new Exception("Achievement not found");
        foreach (var alt in achievementInDb.AchievementLevelThresholds) await fileService.DeleteFileAsync(alt.LogoPath);
        await unitOfWork.GetRepository<AchievementLevelThreshold>()
            .RemoveRangeAsync(achievementInDb.AchievementLevelThresholds);
        await unitOfWork.SaveChangesAsync();

        var achievement = mapper.Map<Achievement>(dto);

        foreach (var alt in achievement.AchievementLevelThresholds)
        {
            var imagePath = await fileService.SaveBase64PngAsync(
                dto.AchievementLevelThresholds.FirstOrDefault(lt => lt.AchievementLevelId == alt.AchievementLevelId)!
                    .LogoBase64, $"{achievement.Category}_{alt.AchievementLevelId}.png");
            alt.LogoPath = imagePath;
        }

        return await base.Update(achievement);
    }

    public async Task<AchievementResultDto> GetByIdWithThresholds(int id)
    {
        var achievement = await unitOfWork.GetRepository<Achievement>().GetAllAsQueryable()
            .Include(a => a.AchievementLevelThresholds).FirstOrDefaultAsync(a => a.Id == id);

        if (achievement == null) throw new Exception("Achievement not found");

        var result = mapper.Map<AchievementResultDto>(achievement);

        foreach (var alt in result.AchievementLevelThresholds)
        {
            var image = await fileService.GetFileAsync($"{achievement.Category}_{alt.AchievementLevelId}.png");
            alt.LogoBase64 = image.Base64;
        }

        return result;
    }
}