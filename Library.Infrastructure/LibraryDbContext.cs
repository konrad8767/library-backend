using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Library.Infrastructure
{
    public class LibraryDbContext : DbContext
    {
        private string _connectionString =
            "Server=PKONDZIK;Database=LibraryDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}