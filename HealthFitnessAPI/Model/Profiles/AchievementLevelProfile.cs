using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.AchievementLevel;
using HealthFitnessAPI.Model.Dtos.AchievementLevelThreshold;

namespace HealthFitnessAPI.Model.Profiles;

public class AchievementLevelProfile : Profile
{
    public AchievementLevelProfile()
    {
        CreateMap<CreateAchievementLevelDto, AchievementLevel>();
        CreateMap<UpdateAchievementLevelDto, AchievementLevel>();
        CreateMap<AchievementLevel, AchievementLevelResultDto>();

        CreateMap<AchievementLevelThresholdDto, AchievementLevelThreshold>();
        CreateMap<AchievementLevelThreshold, AchievementLevelThresholdResultDto>();
    }
}