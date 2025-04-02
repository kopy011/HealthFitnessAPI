using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Helpers;
using HealthFitnessAPI.Model.Dtos.Achievement;
namespace HealthFitnessAPI.Model.Profiles
{
    public class AchievementProfile : Profile
    {
        public AchievementProfile()
        {
            CreateMap<CreateAchievementDto, Achievement>()
                .ForMember(dest => dest.AchievementType, opt => opt.MapFrom(src => EnumHelper.GetAchievementTypeEnumValue(src.AchievementType)));
            CreateMap<UpdateAchievementDto, Achievement>()
                .ForMember(dest => dest.AchievementType, opt => opt.MapFrom(src => EnumHelper.GetAchievementTypeEnumValue(src.AchievementType)));
            CreateMap<Achievement, AchievementResultDto>()
                .ForMember(dest => dest.AchievementType, opt => opt.MapFrom(src => EnumHelper.GetAchievementTypeDisplayValue(src.AchievementType)));
        }
    }
}
