using Library.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Library.API.DTO
{
    public class BookDTO : BaseAuditDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public BookGenre Genres { get; set; }
        public BookStatus Status { get; set; }
        public List<AuthorDTO> Authors { get; set; }
        public int Version { get; set; }
        public DateTime PublicationDate { get; set; }

    }
}
