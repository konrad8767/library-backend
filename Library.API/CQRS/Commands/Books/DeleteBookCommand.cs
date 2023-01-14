using AutoMapper;
using FluentValidation;
using Library.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Library.API.CQRS.Commands.Books
{
    public class DeleteBookCommand
    {
        public class Request : IRequest<Response>
        {
            public int Id { get; set; }
        }

        public class Response
        {

        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationErrorKeys.BookIdInvalid);
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IBookRepository _bookRepository;

            public Handler(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                await _bookRepository.RemoveBookById(request.Id, cancellationToken);
                return new Response();
            }
        }
    }
}
