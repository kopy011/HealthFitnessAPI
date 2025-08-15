using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Helpers;
using HealthFitnessAPI.Model.Dtos.UserAchievement;

namespace HealthFitnessAPI.Model.Profiles;

public class UserAchievementProfile : Profile
{
    public UserAchievementProfile()
    {
        CreateMap<CreateUserAchievementDto, UserAchievement>()
            .ForMember(dest => dest.AchievementLevel,
                opt => opt.MapFrom(src => EnumHelper.GetAchievementLevelEnumValue(src.AchievementLevel)))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        CreateMap<UpdateUserAchievementDto, UserAchievement>()
            .ForMember(dest => dest.AchievementLevel,
                opt => opt.MapFrom(src => EnumHelper.GetAchievementLevelEnumValue(src.AchievementLevel)));
        CreateMap<UserAchievement, UserAchievementResultDto>()
            .ForMember(dest => dest.AchievementLevel,
                opt => opt.MapFrom(src => EnumHelper.GetAchievementLevelDisplayValue(src.AchievementLevel)));
    }
}