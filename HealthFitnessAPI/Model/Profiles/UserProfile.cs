using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Helpers;
using HealthFitnessAPI.Model.Dtos.Friendship;
using HealthFitnessAPI.Model.Dtos.User;

namespace HealthFitnessAPI.Model.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => EnumHelper.GetGenderEnumValue(src.Gender)))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => Hash.HashPassword(src.Password!)));
        CreateMap<UpdateUserDto, User>().ForMember(dest => dest.Gender,
            opt => opt.MapFrom(src => EnumHelper.GetGenderEnumValue(src.Gender)));
        CreateMap<User, UserResultDto>().ForMember(dest => dest.Gender,
            opt => opt.MapFrom(src => EnumHelper.GetGenderDisplayValue(src.Gender)));

        CreateMap<Friendship, FriendshipResultDto>().ForMember(dest => dest.Status,
            opt => opt.MapFrom(src => EnumHelper.GetFriendShipStatusDisplayValue(src.Status)));

        CreateMap<User, UserProfileResultDto>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => EnumHelper.GetGenderDisplayValue(src.Gender)));
        CreateMap<UpdateUserProfileDto, User>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => EnumHelper.GetGenderEnumValue(src.Gender)));
    }
}