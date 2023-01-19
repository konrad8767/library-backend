using Library.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Library.API.DTO
{
    public class BookDTO
    {
        public int? BookId { get; set; }
        public string Title { get; set; }
        public BookGenre Genres { get; set; }
        public BookStatus Status { get; set; }
        public UserDTO User { get; set; }
        public AuthorDTO Author { get; set; }
        public int Version { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ImageUrl { get; set; }

    }
}
