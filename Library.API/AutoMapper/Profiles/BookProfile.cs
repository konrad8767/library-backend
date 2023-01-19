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
                .ForMember(z => z.BookId, x => x.MapFrom(c => c.Id))
                .ForMember(z => z.User, x => x.MapFrom(c => c.User));

            CreateMap<BookDTO, Book>();

            CreateMap<UserDTO, User>();

            CreateMap<User, UserDTO>()
                .ForMember(z => z.UserId, x => x.MapFrom(c => c.Id));

            CreateMap<Book, MyBookDTO>()
                .ForMember(z => z.BookId, x => x.MapFrom(c => c.Id));

            CreateMap<MyBookDTO, Book>();
        }
    }
}
