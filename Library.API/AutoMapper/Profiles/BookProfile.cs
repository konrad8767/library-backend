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
                .ForMember(z => z.BookId, x => x.MapFrom(c => c.Id));

            CreateMap<BookDTO, Book>();
        }
    }
}
