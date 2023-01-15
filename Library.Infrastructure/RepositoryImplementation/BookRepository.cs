using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Domain.Models.Filters;
using Library.Domain.Models.Sorting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Infrastructure.RepositoryImplementation
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IFilterRepository _filterRepository;

        public BookRepository(LibraryDbContext dbContext, IFilterRepository filterRepository)
        {
            _dbContext = dbContext;
            _filterRepository = filterRepository;
        }

        public async Task<Book> GetBookById(int bookId, CancellationToken cancellationToken)
        {
            return await _dbContext.Books
                .Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == bookId, cancellationToken);
        }

        public async Task<int> CreateBook (Book book, CancellationToken cancellationToken)
        {
            await _dbContext.Books.AddAsync(book, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return book.Id;
        }

        public async Task RemoveBookById(int bookId, CancellationToken cancellationToken)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == bookId, cancellationToken);

            if (book is null) return;

            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<ListResult<Book>> SearchBook(IList<SearchFilter> filters, BookSorting sortingField, bool isDesc, CancellationToken cancellationToken)
        {
            var query = _dbContext.Books
                .Include(x => x.Author)
                .Where(x => x.Id != null);

            query = _filterRepository.SetBookFilters(query, filters);
            query = ApplyBookSorting(query, sortingField, isDesc);

            var books = await query.ToListAsync();
            var count = await query.CountAsync();

            return new ListResult<Book>(books, count);
        }

        public async Task<bool> NotExistingBook(int bookId, CancellationToken cancellationToken)
        {
            var bookIdInDb = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == bookId);

            if (bookIdInDb == null) return false;

            return true;
        }

        public async Task UpdateBook(Book book, CancellationToken cancellationToken)
        {
            _dbContext.Books.Update(book);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<Book> ApplyBookSorting(IQueryable<Book> query, BookSorting sortingField, bool isDesc)
            => isDesc
            ? sortingField switch
            {
                BookSorting.Title => query.OrderByDescending(x => x.Title),
                BookSorting.Genres => query.OrderByDescending(x => x.Genres),
                BookSorting.Status => query.OrderByDescending(x => x.Status),
                BookSorting.Author => query.OrderByDescending(x => x.Author),
                BookSorting.Version => query.OrderByDescending(x => x.Version),
                BookSorting.PublicationDate => query.OrderByDescending(x => x.PublicationDate),
                _ => query,
            }
            : sortingField switch
            {
                BookSorting.Title => query.OrderBy(x => x.Title),
                BookSorting.Genres => query.OrderBy(x => x.Genres),
                BookSorting.Status => query.OrderBy(x => x.Status),
                BookSorting.Author => query.OrderBy(x => x.Author),
                BookSorting.Version => query.OrderBy(x => x.Version),
                BookSorting.PublicationDate => query.OrderBy(x => x.PublicationDate),
                _ => query,
            };
    }
}
