using Library.API.CQRS.Queries.Books;
using Library.Domain.Entities;
using Library.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Linq;

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
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Books.Any())
                {
                    var users = GetUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();

                    var books = GetBooks();
                    _dbContext.Books.AddRange(books);
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
                SpectatedBookIds = "",
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
                SpectatedBookIds = "",
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
                    Title = "Władca Pierścieni",
                    Genres = BookGenre.Fantasy | BookGenre.Action,
                    Status = BookStatus.Available,
                    User = null,
                    Author = new Author() { FirstName = "J.R.R.", LastName = "Tolkien" },
                    Version = 1,
                    PublicationDate = new DateTime(1954, 7, 29),
                    ImageUrl = "https://ecsmedia.pl/c/14702679941213326-jpg-gallery.big-iext41561989.jpg",
                },
                new Book()
                {
                    Title = "Harry Potter i Książę Półkrwi",
                    Genres = BookGenre.Action | BookGenre.Romance | BookGenre.Fantasy,
                    Status = BookStatus.Available,
                    User = null,
                    Author = new Author() { FirstName = "J.K.", LastName = "Rowling" },
                    Version = 1,
                    PublicationDate = new DateTime(2005, 7, 16),
                    ImageUrl = "https://image.ceneostatic.pl/data/products/47839675/i-harry-potter-i-ksiaze-polkrwi-tom-6.jpg",
                },
                new Book()
                {
                    Title = "Król Maciuś Pierwszy",
                    Genres = BookGenre.TravelLiterature | BookGenre.Fantasy,
                    Status = BookStatus.Available,
                    User = null,
                    Author = new Author() { FirstName = "Janusz", LastName = "Korczak" },
                    Version = 1,
                    PublicationDate = new DateTime(1922, 1, 1),
                    ImageUrl = "https://fwcdn.pl/fpo/65/38/456538/7195878.3.jpg",
                },
                new Book()
                {
                    Title = "Lalka",
                    Genres = BookGenre.TravelLiterature | BookGenre.Romance,
                    Status = BookStatus.Available,
                    User = null,
                    Author = new Author() { FirstName = "Bolesław", LastName = "Prus" },
                    Version = 1,
                    PublicationDate = new DateTime(1889, 1, 1),
                    ImageUrl = "https://image.ceneostatic.pl/data/products/50089449/i-lalka.jpg"
                }
            };

            return books;
        }
    }
}
