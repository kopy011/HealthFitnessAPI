using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.Achievement;

namespace HealthFitnessAPI.Model.Profiles;

public class AchievementProfile : Profile
{
    public AchievementProfile()
    {
        CreateMap<CreateAchievementDto, Achievement>();
        CreateMap<UpdateAchievementDto, Achievement>();
        CreateMap<Achievement, AchievementResultDto>();
        CreateMap<Achievement, AchievementPathOnlyResultDto>();
    }
}