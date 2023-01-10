using AutoMapper;
using Library.API.DTO;
using Library.Domain.Entities;

namespace Library.API.AutoMapper.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDTO>()
                .ReverseMap();
        }
    }
}
