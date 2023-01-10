using Library.API.CQRS.Queries.Books;
using Library.Domain.Entities;
using Library.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Library.API
{
    public class LibrarySeeder
    {
        private readonly LibraryDbContext _dbContext;

        public LibrarySeeder(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if(_dbContext.Database.CanConnect())
            {
                if(!_dbContext.Books.Any())
                {
                    var books = GetBooks();
                    _dbContext.Books.AddRange(books);
                    _dbContext.SaveChanges();
                }
            }
        }

        private List<Book> GetBooks()
        {
            var books = new List<Book>()
            {
                new Book()
                {
                    Title = "Harry Potter and the Half-Blood Prince",
                    Genres = BookGenre.Action | BookGenre.Romance | BookGenre.Fantasy,
                    Status = BookStatus.Available,
                    Authors = new List<Author>() {
                        new Author() { FirstName = "J.K.", LastName = "Rowling" }, 
                        new Author() { FirstName = "Mary", LastName = "GrandPré"}
                    },
                    Version = 1,
                    PublicationDate = new DateTime(2005, 7, 16)
                },
                new Book()
                {
                    Title = "Harry Potter and the Half-Blood Prince",
                    Genres = BookGenre.Action | BookGenre.Romance | BookGenre.Fantasy,
                    Status = BookStatus.Available,
                    Authors = new List<Author>() {
                        new Author() { FirstName = "J.K.", LastName = "Rowling" },
                        new Author() { FirstName = "Mary", LastName = "GrandPré"}
                    },
                    Version = 2,
                    PublicationDate = new DateTime(2005, 7, 16)
                },
                new Book()
                {
                    Title = "Harry Potter and the Half-Blood Prince",
                    Genres = BookGenre.Action | BookGenre.Romance | BookGenre.Fantasy,
                    Status = BookStatus.Available,
                    Authors = new List<Author>() {
                        new Author() { FirstName = "J.K.", LastName = "Rowling" },
                        new Author() { FirstName = "Mary", LastName = "GrandPré"}
                    },
                    Version = 3,
                    PublicationDate = new DateTime(2005, 7, 16)
                },
                new Book()
                {
                    Title = "In a Sunburned Country",
                    Genres = BookGenre.TravelLiterature,
                    Status = BookStatus.Available,
                    Authors = new List<Author>() {new Author() { FirstName = "Bill", LastName = "Bryson" } },
                    Version = 1,
                    PublicationDate = new DateTime(2000, 6, 6)
                }
            };

            return books;
        }
    }
}
