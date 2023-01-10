using Library.Domain.Entities;
using System.Collections.Generic;

namespace Library.API.DTO
{
    public class AuthorDTO : BaseAuditDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<Book> Books { get; set; }

    }
}
