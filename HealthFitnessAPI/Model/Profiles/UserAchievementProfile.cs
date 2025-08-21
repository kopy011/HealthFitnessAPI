using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.UserAchievement;

namespace HealthFitnessAPI.Model.Profiles;

public class UserAchievementProfile : Profile
{
    public UserAchievementProfile()
    {
        CreateMap<CreateUserAchievementDto, UserAchievement>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        CreateMap<UpdateUserAchievementDto, UserAchievement>();
        CreateMap<UserAchievement, UserAchievementResultDto>()
            .ForMember(dest => dest.AchievementLevelName, opt => opt.MapFrom(src => src.AchievementLevel!.Name));
    }
}