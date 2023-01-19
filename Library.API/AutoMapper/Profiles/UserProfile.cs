using AutoMapper;
using Library.API.DTO;
using Library.Domain.Entities;

namespace Library.API.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserRegisterDTO>()
                .ReverseMap();

            CreateMap<User, UserLoginDTO>()
                .ForMember(z => z.UserId, x => x.MapFrom(c => c.Id));

            CreateMap<UserLoginDTO, User>();
        }
    }
}
