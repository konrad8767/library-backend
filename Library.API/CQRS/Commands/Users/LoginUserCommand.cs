using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Library.API.CQRS.Queries.Books;
using Library.API.DTO;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Library.API.CQRS.Commands.Users
{
    public class LoginUserCommand
    {
        public class Request : IRequest<Response>
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }

        public class Response
        {
            public string Token { get; set; }
            public bool IsAdmin { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {

        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly IPasswordHasher<User> _passwordHasher;
            private readonly IConfiguration _configuration;

            public Handler(IUserRepository userRepository, IMapper mapper, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _passwordHasher = passwordHasher;
                _configuration = configuration;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = _userRepository.GetUserByLogin(request.Login.ToLower(), cancellationToken).Result;

                if (user is null)
                    return new Response();

                var isPasswordVerified = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

                if (isPasswordVerified == PasswordVerificationResult.Failed)
                    return new Response();

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expireDate = DateTime.Now.AddDays(15);
                var token = new JwtSecurityToken( _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: expireDate,
                    signingCredentials: credentials);
                var tokenHandler = new JwtSecurityTokenHandler();

                return new Response
                {
                    Token = tokenHandler.WriteToken(token),
                    IsAdmin = user.Role.Name == "Admin"
                };
            }   
        }

    }
}
