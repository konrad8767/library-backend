using AutoMapper;
using Library.API.DTO;
using Library.Domain.Entities;

namespace Library.API.AutoMapper.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDTO>()
                .ReverseMap();
        }
    }
}
