using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.User;
namespace HealthFitnessAPI.Model.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UserResultDto>();
        }
    }
}
