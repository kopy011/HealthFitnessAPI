using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.AchievementLevel;
using HealthFitnessAPI.UnitOfWork;

namespace HealthFitnessAPI.Services;

public interface IAchievementLevelService : IAbstractService<AchievementLevel>
{
    Task<AchievementLevel> CreateWithUpload(CreateAchievementLevelDto dto);
    Task<AchievementLevel> UpdateWithUpload(UpdateAchievementLevelDto dto);
    Task<List<AchievementLevelResultDto>> GetAllWithImages();
}

public class AchievementLevelService(IUnitOfWork unitOfWork, IFileService fileService, IMapper mapper)
    : AbstractService<AchievementLevel>(unitOfWork), IAchievementLevelService
{
    public async Task<AchievementLevel> CreateWithUpload(CreateAchievementLevelDto dto)
    {
        var imagePath = await fileService.SaveBase64PngAsync(dto.BadgeBase64, $"{dto.Name}.png");
        var result = await base.Create(new AchievementLevel
        {
            LogoFilePath = imagePath,
            Name = dto.Name
        });

        return result;
    }

    public async Task<AchievementLevel> UpdateWithUpload(UpdateAchievementLevelDto dto)
    {
        var levelInDb = await GetById(dto.Id);
        await fileService.DeleteFileAsync(levelInDb.LogoFilePath);

        var imagePath = await fileService.SaveBase64PngAsync(dto.BadgeBase64, $"{dto.Name}.png");
        var result = await base.Update(new AchievementLevel
        {
            LogoFilePath = imagePath,
            Name = dto.Name
        });

        return result;
    }

    public async Task<List<AchievementLevelResultDto>> GetAllWithImages()
    {
        var all = await base.GetAll();

        var results = await Task.WhenAll(all.Select(async al =>
        {
            var file = await fileService.GetFileAsync(al.Name);
            return new AchievementLevelResultDto
            {
                Id = al.Id,
                Name = al.Name,
                BadgeBase64 = file.Base64
            };
        }));

        return results.ToList();
    }
}