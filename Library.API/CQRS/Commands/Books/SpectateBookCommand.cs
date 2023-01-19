using FluentValidation;
using Library.Domain.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Library.Domain.Entities;

namespace Library.API.CQRS.Commands.Books
{
    public class SpectateBookCommand
    {
        public class Request : IRequest<Response>
        {
            public int BookId { get; set; }
            public int UserId { get; set; }
        }

        public class Response
        {
            public int BookId { get; set; }
            public bool Success { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.BookId).GreaterThan(0)
                    .WithMessage(ValidationErrorKeys.BookIdInvalid);
                RuleFor(x => x.UserId).GreaterThan(0)
                    .WithMessage(ValidationErrorKeys.UserIdInvalid);
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IBookRepository _bookRepository;
            private readonly IUserRepository _userRepository;
            //private INotificationRepostiory _notificationRepository;

            public Handler(IBookRepository bookRepository, IUserRepository userRepository)
            {
                _bookRepository = bookRepository;
                _userRepository = userRepository;
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

                if (book.Status != BookStatus.Borrowed)
                    return new Response()
                    {
                        BookId = request.BookId,
                        Success = false
                    };

                var user = await _userRepository.GetUserById(request.UserId, cancellationToken);

                var userSpectatedBookIds = user.SpectatedBookIds;

                if (userSpectatedBookIds.IsNullOrEmpty())
                    userSpectatedBookIds = book.Id.ToString();
                else
                {
                    if (userSpectatedBookIds.Contains(book.Id.ToString()))
                    {
                        return new Response()
                        {
                            BookId = request.BookId,
                            Success = false
                        };
                    }
                    userSpectatedBookIds += "," + book.Id.ToString();
                }
                
                user.SpectatedBookIds = userSpectatedBookIds;
                await _userRepository.UpdateUser(user, cancellationToken);

                return new Response()
                {
                    BookId = request.BookId,
                    Success = true
                };
            }
        }
    }
}
