using Library.Domain.Entities;
using Library.Domain.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Domain.Interfaces
{
    public interface IFilterRepository
    {
        IList<AvailableFilter> GetAvailableFiltersForBooks();
        IQueryable<Book> SetBookFilters(IQueryable<Book> query, IList<SearchFilter> filters);
        IQueryable<Book> SetBookFilter(IQueryable<Book> query, SearchFilter filter);
    }
}
