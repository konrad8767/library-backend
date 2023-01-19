using Library.Domain.Entities;
using System.Collections.Generic;

namespace Library.API.DTO
{
    public class UserDTO
    {
        public int? UserId { get; set; }
        public string Login { get; set; }
        public string EmailAddress { get; set; }
        public string SpectatedBookIds { get; set; }
    }
}
