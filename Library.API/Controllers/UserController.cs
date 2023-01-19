using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Library.API.DTO;
using Library.Domain.Entities;
using Library.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Library.API.CQRS.Queries.Books;
using Library.API.CQRS.Commands.Users;
using System.Net;
using System.Threading;
using Library.Domain.Interfaces;
using Library.API.CQRS.Commands.Books;
using MediatR;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly IPasswordHasher<User> _passwordHasher;
        public IConfiguration Configuration { get; }

        public UserController(LibraryDbContext dbContext, IPasswordHasher<User> passwordHasher, IConfiguration configuration, IUserRepository userRepository, IMediator mediator)
        {
            _passwordHasher = passwordHasher;
            _dbContext = dbContext;
            Configuration = configuration;
            _userRepository = userRepository;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(LoginUserCommand.Response), 200)]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO user, CancellationToken cancellationToken)
        {
            var request = new LoginUserCommand.Request
            {
                Login = user.Login,
                Password = user.Password
            };

            var result = await _mediator.Send(request, cancellationToken);

            if (result.Token == null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(CreateUserCommand.Response), 200)]
        [Route("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterDTO user, CancellationToken cancellationToken)
        {
            var request = new CreateUserCommand.Request
            {
                User = user
            };

            var result = await _mediator.Send(request, cancellationToken);

            if (!ModelState.IsValid)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
