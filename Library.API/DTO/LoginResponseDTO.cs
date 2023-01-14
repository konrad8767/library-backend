using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.SagaStateMachine;

namespace Library.API.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public bool IsAdmin { get; set; }
    }
}
