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

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        public IConfiguration Configuration { get; }

        public UserController(LibraryDbContext dbContext, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _passwordHasher = passwordHasher;
            _dbContext = dbContext;
            Configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLoginDTO userLogin)
        {
            var user = Authenticate(userLogin);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("createUser")]
        public IActionResult CreateUser([FromBody] CreateUserDTO user)
        {
            var isUserCreated = CreateNewUser(user);
            if (isUserCreated)
            {
                return Ok();
            }
            return BadRequest();
        }


        private bool CreateNewUser(CreateUserDTO createUser)
        {
            var user = _dbContext.Users.FirstOrDefault(user => createUser.Username == user.Login);
            if (user is null)
            {
                var newUser = new User()
                {
                    Role = new Role()
                    {
                        Name = "User"
                    },
                    Login = createUser.Username,
                    Password = createUser.Password,
                    EmailAddress = createUser.Email
                };

                if (_dbContext.Database.CanConnect())
                {
                    _dbContext.Users.Add(newUser);
                    _dbContext.SaveChanges();
                }

                return true;
            }

            return false;
        }

        private LoginResponseDTO Authenticate(UserLoginDTO userLogin)
        {
            var user = _dbContext.Users.Include(x => x.Role).FirstOrDefault(u => u.Login == userLogin.Username);
            if (user is null)
            {
                return null;
            }
            if (_passwordHasher.VerifyHashedPassword(user, user.Password, userLogin.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireDate = DateTime.Now.AddDays(15);
            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"], Configuration["Jwt:Audience"], claims, expires: expireDate,
                signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var loginResponseDto = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                IsAdmin = user.Role.Name == "Admin"
            };

            return loginResponseDto;
        }
    }
}
