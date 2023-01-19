using FluentValidation;
using Library.Domain.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Library.API.CQRS.Commands.Books
{
    public class ReturnBookCommand
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

                var book = await _bookRepository.GetBookById(request.BookId, cancellationToken);
                var updatedBook = book.Return();

                await _bookRepository.UpdateBook(updatedBook, cancellationToken);
                //await _userRepository.RemoveSpectatedBook(book.Id, cancellationToken);

                return new Response()
                {
                    BookId = request.BookId,
                    Success = true
                };
            }
        }
    }
}
