using Library.Domain.Entities;
using Library.Domain.Models.Filters;
using Library.Domain.Models.Sorting;
using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> GetBookById(int bookId, CancellationToken cancellationToken);
        Task<ListResult<Book>> SearchBook(IList<SearchFilter> filters, BookSorting sortingField, bool isDesc, CancellationToken cancellationToken);
    }
}
