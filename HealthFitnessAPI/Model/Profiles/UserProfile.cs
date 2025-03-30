using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Helpers;
using HealthFitnessAPI.Model.Dtos.User;
namespace HealthFitnessAPI.Model.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>().ForMember(dest => dest.Gender, opt => opt.MapFrom(src => EnumHelper.GetGenderEnumValue(src.Gender)));
            CreateMap<UpdateUserDto, User>().ForMember(dest => dest.Gender, opt => opt.MapFrom(src => EnumHelper.GetGenderEnumValue(src.Gender)));
            CreateMap<User, UserResultDto>().ForMember(dest => dest.Gender, opt => opt.MapFrom(src => EnumHelper.GetGenderDisplayValue(src.Gender)));
        }
    }
}
