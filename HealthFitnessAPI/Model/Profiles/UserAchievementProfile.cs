using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.UserAchievement;
namespace HealthFitnessAPI.Model.Profiles
{
    public class UserAchievementProfile : Profile
    {
        public UserAchievementProfile()
        {
            CreateMap<CreateUserAchievementDto, UserAchievement>();
            CreateMap<UpdateUserAchievementDto, UserAchievement>();
            CreateMap<UserAchievement, UserAchievementResultDto>();
        }
    }
}
