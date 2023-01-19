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
        public User User { get; set; }
        public Author Author { get; set; }
        public int Version { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ImageUrl { get; set; }

        public void Update(Book other)
        {
            Title = other.Title;
            Genres = other.Genres;
            User = other.User;
            Author = other.Author;
            Version = other.Version;
            PublicationDate = other.PublicationDate;
            ImageUrl = other.ImageUrl;
        }

        public Book Borrow(User other)
        {
            Status = BookStatus.Borrowed;
            User = other;

            return this;
        }

        public Book Return()
        {
            Status = BookStatus.Available;
            User = null;

            return this;
        }
    }
}
