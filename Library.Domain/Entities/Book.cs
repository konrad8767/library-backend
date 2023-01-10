using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public BookGenre Genres { get; set; }
        public BookStatus Status { get; set; }
        public virtual List<Author> Authors { get; set; }
        public int Version { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
