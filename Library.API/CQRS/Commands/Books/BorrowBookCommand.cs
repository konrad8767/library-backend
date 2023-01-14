using FluentValidation;
using Library.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Library.API.CQRS.Commands.Books
{
    public class BorrowBookCommand
    {
        public class Request : IRequest<Response>
        {
            public int BookId { get; set; }
        }

        public class Response
        {
            public int BookId { get; set; }
            public bool Success { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            private readonly IBookRepository _bookRepository;
            public Validator(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;

                RuleFor(x => x.BookId)
                    .GreaterThan(0)
                    .WithMessage(ValidationErrorKeys.BookIdInvalid);
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IBookRepository _bookRepository;
            //private INotificationRepostiory _notificationRepository;

            public Handler(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var notExist = await _bookRepository.NotExistingBook(request.BookId, cancellationToken);

                if (!notExist)
                    return new Response()
                    {
                        BookId = request.BookId,
                        Success = false
                    };

                var book = _bookRepository.GetBookById(request.BookId, cancellationToken);
                var updatedBook = book.Result.Borrow();

                await _bookRepository.UpdateBook(updatedBook, cancellationToken);

                return new Response()
                {
                    BookId = request.BookId,
                    Success = true
                };
            }
        }
    }
}
