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
                .Include(x => x.Authors)
                .FirstOrDefaultAsync(x => x.Id == bookId, cancellationToken);
        }

        public async Task<ListResult<Book>> SearchBook(IList<SearchFilter> filters, BookSorting sortingField, bool isDesc, CancellationToken cancellationToken)
        {
            var query = _dbContext.Books
                .Include(x => x.Authors)
                .Where(x => x.Id != null);

            query = _filterRepository.SetBookFilters(query, filters);
            query = ApplyBookSorting(query, sortingField, isDesc);

            var books = await query.ToListAsync();
            var count = await query.CountAsync();

            return new ListResult<Book>(books, count);
        }

        public IQueryable<Book> ApplyBookSorting(IQueryable<Book> query, BookSorting sortingField, bool isDesc)
            => isDesc
            ? sortingField switch
            {
                BookSorting.Title => query.OrderByDescending(x => x.Title),
                BookSorting.Genres => query.OrderByDescending(x => x.Genres),
                BookSorting.Status => query.OrderByDescending(x => x.Status),
                BookSorting.Authors => query.OrderByDescending(x => x.Authors),
                BookSorting.Version => query.OrderByDescending(x => x.Version),
                BookSorting.PublicationDate => query.OrderByDescending(x => x.PublicationDate),
                _ => query,
            }
            : sortingField switch
            {
                BookSorting.Title => query.OrderBy(x => x.Title),
                BookSorting.Genres => query.OrderBy(x => x.Genres),
                BookSorting.Status => query.OrderBy(x => x.Status),
                BookSorting.Authors => query.OrderBy(x => x.Authors),
                BookSorting.Version => query.OrderBy(x => x.Version),
                BookSorting.PublicationDate => query.OrderBy(x => x.PublicationDate),
                _ => query,
            };
    }
}
