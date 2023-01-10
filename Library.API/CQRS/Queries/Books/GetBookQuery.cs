using AutoMapper;
using FluentValidation;
using Library.API.DTO;
using Library.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Library.API.CQRS.Queries.Books
{
    public static class GetBookQuery
    {
        public class Request : IRequest<Response>
        {
            public int BookId { get; set; }
        }

        public class Response
        {
            public BookDTO Book { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            private readonly IBookRepository _bookRepository;
            public Validator(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
                RuleFor(x => x.BookId).GreaterThan(0).WithMessage(ValidationErrorKeys.BookIdInvalid);
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
                var result = await _bookRepository.GetBookById(request.BookId, cancellationToken);

                if (result is null)
                    return new Response();

                return new Response
                {
                    Book = _mapper.Map<BookDTO>(result)
                };
            }
        }
    }
}
