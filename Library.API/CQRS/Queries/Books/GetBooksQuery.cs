using AutoMapper;
using FluentValidation;
using Library.API.DTO;
using Library.Domain.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using FluentValidation.Validators;
using Library.Domain.Models.Filters;
using System.Linq;
using Library.Domain.Models.Sorting;

namespace Library.API.CQRS.Queries.Books
{
    public class GetBooksQuery
    {
        public class Request : IRequest<Response>
        {
            public IList<SearchFilter> Filters { get; set; }
            public BookSorting SortingField { get; set; }
            public bool IsDesc { get; set; }
        }

        public class Response
        {
            public List<BookDTO> Books { get; set; }
            public int Count { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            private readonly IBookRepository _bookRepository;
            private readonly IFilterRepository _filterRepository;
            public Validator(IFilterRepository filterRepository, IBookRepository bookRepository)
            {
                _filterRepository = filterRepository;
                _bookRepository = bookRepository;
                RuleFor(x => x)
                    .CustomAsync(FiltersAvailable);
            }

            public async Task FiltersAvailable(Request query, CustomContext customContext, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;
                var availableFilters = _filterRepository.GetAvailableFiltersForBooks();

                foreach (var filter in query.Filters)
                {
                    var current = availableFilters.FirstOrDefault(x => x.Property == filter.Property);
                    if (current is null)
                    {
                        customContext.AddFailure("Filter.Property", ValidationErrorKeys.FilterNotAllowed);
                        return;
                    }

                    if (!current.Conditions.Contains(filter.Condition))
                    {
                        customContext.AddFailure("Filter.Condition", ValidationErrorKeys.ConditionNotAllowed);
                        return;
                    }
                }
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IBookRepository _bookRepository;
            private readonly IMapper _mapper;

            public Handler(IBookRepository bookRepository, IMapper mapper)
            {
                _bookRepository = bookRepository;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var result = await _bookRepository.SearchBook(request.Filters, request.SortingField, request.IsDesc, cancellationToken);

                return new Response
                {
                    Books = _mapper.Map<List<BookDTO>>(result.Value).ToList(),
                    Count = result.Count
                };
            }
        }
    }
}
