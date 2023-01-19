using AutoMapper;
using FluentValidation;
using FluentValidation.Validators;
using Library.API.DTO;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.API.CQRS.Commands.Users
{
    public class CreateUserCommand
    {
        public class Request : IRequest<Response>
        {
            public UserRegisterDTO User { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            private readonly IUserRepository _userRepository;

            public Validator(IUserRepository userRepository)
            {
                _userRepository = userRepository;

                RuleFor(x => x)
                    .CustomAsync(LoginIsUnique)
                    .CustomAsync(EmailIsUnique);
            }

            private async Task LoginIsUnique(Request request, CustomContext customContext, CancellationToken cancellationToken)
            {
                var isUnique = await _userRepository.IsUserLoginUnique(request.User.Login.Trim().ToLower(), cancellationToken);

                if (!isUnique)
                    customContext.AddFailure("UserLogin", ValidationErrorKeys.UserLoginIsNotUnique);
            }

            private async Task EmailIsUnique(Request request, CustomContext customContext, CancellationToken cancellationToken)
            {
                var emailExist = await _userRepository.IsUserEmailUnique(request.User.EmailAddress, cancellationToken);

                if (emailExist)
                    customContext.AddFailure("Email", ValidationErrorKeys.EmailIsNotUnique);
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IUserRepository _userRepository;
            private readonly IPasswordHasher<User> _passwordHasher;
            private readonly IMapper _mapper;

            public Handler(IUserRepository userRepository, IMapper mapper, IPasswordHasher<User> passwordHasher)
            {
                _userRepository = userRepository;
                _passwordHasher = passwordHasher;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = _mapper.Map<User>(request.User);
                user.RoleId = 2;
                user.Password = _passwordHasher.HashPassword(user, user.Password);

                var result = await _userRepository.CreateUser(user, cancellationToken);
                return new Response { Id = result };
            }
        }
    }
}
