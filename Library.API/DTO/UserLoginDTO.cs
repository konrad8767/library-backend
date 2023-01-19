using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.DTO
{
    public class UserLoginDTO
    {
        public int? UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
