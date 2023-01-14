using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.API.CQRS.Queries.Books;
using MediatR;

namespace Library.API.CQRS.Commands.Users
{
    public class LoginUserCommand
    {
        public class Request : IRequest<Response>
        {
            public int BookId { get; set; }
        }

        public class Response
        {
            public string Token { get; set; }
            public bool IsAdmin { get; set; }
        }

    }
}
