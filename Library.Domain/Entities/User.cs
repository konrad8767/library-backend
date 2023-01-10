using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        public virtual List<Book> Books { get; set; }
        public virtual Role Role { get; set; }
    }
}
