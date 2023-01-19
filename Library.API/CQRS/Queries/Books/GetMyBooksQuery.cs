using AutoMapper;
using FluentValidation.Validators;
using FluentValidation;
using Library.API.DTO;
using Library.Domain.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Library.Infrastructure.RepositoryImplementation;
using System.Linq;
using System;
using Microsoft.IdentityModel.Tokens;

namespace Library.API.CQRS.Queries.Books
{
    public class GetMyBooksQuery
    {
        public class Request : IRequest<Response>
        {
            public int UserId { get; set; }
        }

        public class Response
        {
            public List<MyBookDTO> Books { get; set; }
            public int Count { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            private readonly IUserRepository _userRepository;
            public Validator(IUserRepository userRepository)
            {
                _userRepository = userRepository;
                RuleFor(x => x.UserId).GreaterThan(0).WithMessage(ValidationErrorKeys.UserIdInvalid);
                RuleFor(x => x)
                    .CustomAsync(UserExist);
            }

            private async Task UserExist(Request request, CustomContext customContext, CancellationToken cancellationToken)
            {
                var userInDb = await _userRepository.IsUserInDb(request.UserId, cancellationToken);

                if (!userInDb)
                    customContext.AddFailure("UserId", ValidationErrorKeys.UserIdInvalid);
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IUserRepository _userRepository;
            private readonly IBookRepository _bookRepository;
            private readonly IMapper _mapper;

            public Handler(IUserRepository userRepository, IBookRepository bookRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _bookRepository = bookRepository;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetUserById(request.UserId, cancellationToken);
                
                if (user.SpectatedBookIds.IsNullOrEmpty())
                    return new Response();

                var bookIds = user.SpectatedBookIds?.Split(',')?.Select(Int32.Parse)?.ToList();
                
                if (bookIds.IsNullOrEmpty())
                {
                    return new Response();
                }

                var result = await _bookRepository.GetBooksByIds(bookIds, cancellationToken);
                var count = result.Count();

                if (result is null)
                    return new Response();

                return new Response
                {
                    Books = _mapper.Map<List<MyBookDTO>>(result),
                    Count = count
                };
            }
        }
    }
}
