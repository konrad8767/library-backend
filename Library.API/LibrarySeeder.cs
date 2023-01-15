using Library.API.CQRS.Queries.Books;
using Library.Domain.Entities;
using Library.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Library.API
{
    public class LibrarySeeder
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LibrarySeeder(LibraryDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public void Seed()
        {
            if(_dbContext.Database.CanConnect())
            {
                if(!_dbContext.Books.Any())
                {
                    var books = GetBooks();
                    _dbContext.Books.AddRange(books);
                    _dbContext.Users.AddRange(GetUsers());
                    _dbContext.SaveChanges();
                }
            }
        }


        private List<User> GetUsers()
        {

            var user = new User()
            {
                Login = "user",
                Role = new Role()
                {
                    Name = "User"
                },
                EmailAddress = "user@test.pl",
            };
            var hashedPassword = _passwordHasher.HashPassword(user, "user");
            user.Password = hashedPassword;
            var admin = new User()
            {
                Login = "admin",
                Role = new Role()
                {
                    Name = "Admin"
                },
                EmailAddress = "user@test.pl",
            };
            var adminHashedPassword = _passwordHasher.HashPassword(user, "admin");
           admin.Password = adminHashedPassword;
           return new List<User>()
           {
               admin, user
           };
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
                    Author = new Author() { FirstName = "J.K.", LastName = "Rowling" },
                    Version = 1,
                    PublicationDate = new DateTime(2005, 7, 16),
                    ImageUrl = "http://universe.byu.edu/wp-content/uploads/2015/01/HP4cover.jpg"
                },
                new Book()
                {
                    Title = "Harry Potter and the Half-Blood Prince",
                    Genres = BookGenre.Action | BookGenre.Romance | BookGenre.Fantasy,
                    Status = BookStatus.Available,
                    Author = new Author() { FirstName = "J.K.", LastName = "Rowling" },
                    Version = 2,
                    PublicationDate = new DateTime(2005, 7, 16),
                    ImageUrl = "http://universe.byu.edu/wp-content/uploads/2015/01/HP4cover.jpg"
                },
                new Book()
                {
                    Title = "Harry Potter and the Half-Blood Prince",
                    Genres = BookGenre.Action | BookGenre.Romance | BookGenre.Fantasy,
                    Status = BookStatus.Available,
                    Author = new Author() { FirstName = "J.K.", LastName = "Rowling" },
                    Version = 3,
                    PublicationDate = new DateTime(2005, 7, 16),
                    ImageUrl = "http://universe.byu.edu/wp-content/uploads/2015/01/HP4cover.jpg"
                },
                new Book()
                {
                    Title = "In a Sunburned Country",
                    Genres = BookGenre.TravelLiterature,
                    Status = BookStatus.Available,
                    Author = new Author() { FirstName = "Bill", LastName = "Bryson" },
                    Version = 1,
                    PublicationDate = new DateTime(2000, 6, 6),
                    ImageUrl = "https://m.media-amazon.com/images/I/511w+m1L4fL._AC_SY1000_.jpg"
                }
            };

            return books;
        }
    }
}
