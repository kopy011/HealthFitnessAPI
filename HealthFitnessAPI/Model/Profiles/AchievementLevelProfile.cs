using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.AchievementLevel;

namespace HealthFitnessAPI.Model.Profiles;

public class AchievementLevelProfile : Profile
{
    public AchievementLevelProfile()
    {
        CreateMap<CreateAchievementLevelDto, AchievementLevel>();
        CreateMap<UpdateAchievementLevelDto, AchievementLevel>();
        CreateMap<AchievementLevel, AchievementLevelResultDto>();
    }
}