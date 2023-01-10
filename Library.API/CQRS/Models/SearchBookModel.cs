using Library.Domain.Models.Filters;
using Library.Domain.Models.Sorting;
using System.Collections.Generic;

namespace Library.API.CQRS.Models
{
    public class SearchBookModel
    {
        public IList<SearchFilter> Filters { get; set; }
        public BookSorting SortingField { get; set; }
        public bool IsDesc { get; set; }
    }
}
