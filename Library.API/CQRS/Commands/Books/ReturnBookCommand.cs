using FluentValidation;
using Library.Domain.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using Library.Infrastructure.RepositoryImplementation;

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
            private readonly IUserRepository _userRepository;
            private readonly INofiticationRepository _notificationRepository;

            public Handler(IBookRepository bookRepository, IUserRepository userRepository, INofiticationRepository nofiticationRepository)
            {
                _bookRepository = bookRepository;
                _userRepository = userRepository;
                _notificationRepository = nofiticationRepository;
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

                var bookSpectators = await _userRepository.GetSpectatorsByBookId(book.Id, cancellationToken);

                foreach (var spectator in bookSpectators)
                {
                    if (spectator.SpectatedBookIds.Split(',').Length == 1)
                        spectator.SpectatedBookIds = "";
                    spectator.SpectatedBookIds = Regex.Replace(spectator.SpectatedBookIds, $"^({book.Id}),|,({book.Id}),|,({book.Id})$", "");
                }

                await _notificationRepository.SendNotificationStatus(book, cancellationToken);

                await _bookRepository.UpdateBook(updatedBook, cancellationToken);
                await _userRepository.UpdateUsers(bookSpectators, cancellationToken);

                return new Response()
                {
                    BookId = request.BookId,
                    Success = true
                };
            }
        }
    }
}
