using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.AchievementLevelThreshold;

namespace HealthFitnessAPI.Model.Profiles;

public class AchievementLevelThresholdProfile : Profile
{
    public AchievementLevelThresholdProfile()
    {
        CreateMap<CreateAchievementLevelThresholdDto, AchievementLevelThreshold>();
        CreateMap<UpdateAchievementLevelThresholdDto, AchievementLevelThreshold>();
        CreateMap<AchievementLevelThreshold, AchievementLevelThresholdResultDto>()
            .ForMember(dest => dest.AchievementLevelName,
                opt => opt.MapFrom(src => src.AchievementLevel == null ? "" : src.AchievementLevel.Name));
        CreateMap<AchievementLevelThreshold, AchievementLevelThresholdResultWithImageDto>();
    }
}