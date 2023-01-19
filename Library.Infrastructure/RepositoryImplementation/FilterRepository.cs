using Library.Domain.Entities;
using Library.Domain.Extensions;
using Library.Domain.Interfaces;
using Library.Domain.Models.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Infrastructure.RepositoryImplementation
{
    public class FilterRepository : IFilterRepository
    {
        private readonly LibraryDbContext _dbContext;

        public FilterRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<AvailableFilter> GetAvailableFiltersForBooks() => new List<AvailableFilter>
        {
            new AvailableFilter(FilterProperty.TITLE, FilterType.TEXT),
            new AvailableFilter(FilterProperty.GENRES, FilterType.LIST),
            new AvailableFilter(FilterProperty.STATUS, FilterType.NUMBER),
            new AvailableFilter(FilterProperty.AUTHORS, FilterType.LIST),
            new AvailableFilter(FilterProperty.VERSION, FilterType.NUMBER),
            new AvailableFilter(FilterProperty.PUBLICATION_DATE, FilterType.DATE),
        };

        public IQueryable<Book> SetBookFilters(IQueryable<Book> query, IList<SearchFilter> filters)
        {
            foreach (var filter in filters)
            {
                query = SetBookFilter(query, filter);
            }
            return query;
        }

        public IQueryable<Book> SetBookFilter(IQueryable<Book> query, SearchFilter filter)
        {
            if (filter.Property == FilterProperty.TITLE)
                return filter.Condition switch
                {
                    Condition.Equal => query.Where(x => x.Title.ToUpper() == filter.Value.ToStrUp()),
                    Condition.Contains => query.Where(x => EF.Functions.Like(x.Title.ToUpper(), $"%{filter.Value.ToStrUp()}%")),
                    _ => query
                };

            if (filter.Property == FilterProperty.GENRES)
            {
                var genres = GetValuesList(filter.Value);
                return filter.Condition switch
                {
                    Condition.Multiselect => query.Where(x => genres.Contains((int)x.Genres)),
                    _ => query
                };
            } 

            if (filter.Property == FilterProperty.STATUS)
            {
                var statuses = GetValuesList(filter.Value);
                return filter.Condition switch
                {
                    Condition.Multiselect => query.Where(x => statuses.Contains((int)x.Status)),
                    _ => query
                };
            }

            //if (filter.Property == FilterProperty.AUTHORS)
            //    return filter.Condition switch
            //    {
            //        _ => query
            //    };

            if (filter.Property == FilterProperty.VERSION)
            {
                var versions = GetValuesList(filter.Value);
                return filter.Condition switch
                {
                    Condition.Equal => query.Where(x => x.Version == versions.FirstOrDefault()),
                    Condition.GreaterThan => query.Where(x => x.Version > versions.FirstOrDefault()),
                    Condition.LesserThan => query.Where(x => x.Version < versions.FirstOrDefault()),
                    _ => query
                };
            }

            if (filter.Property == FilterProperty.PUBLICATION_DATE)
                return filter.Condition switch
                {
                    Condition.Equal => query.Where(x => x.PublicationDate >= filter.Value.ToDate() && x.PublicationDate <= filter.Value.ToDate().EndOfDay()),
                    Condition.GreaterThan => query.Where(x => x.PublicationDate > filter.Value.ToDateTime().EndOfDay()),
                    Condition.LesserThan => query.Where(x => x.PublicationDate < filter.Value.ToDateTime().EndOfDay()),
                    _ => query
                };

            return query;
        }

        private int[] GetValuesList(object value)
        {
            return string.IsNullOrWhiteSpace(value.ToStrUp())
                ? new int[0]
                : value.ToStrUp().Split(';').Select(x => int.Parse(x)).ToArray();
        }
    }
}
