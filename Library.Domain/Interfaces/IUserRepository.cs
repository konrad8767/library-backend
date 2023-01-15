using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Library.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CreateUser(User createdUser, CancellationToken cancellationToken);
        Task<User> GetUserByLogin(string login, CancellationToken cancellationToken);
        Task<bool> IsUserLoginUnique(string login, CancellationToken cancellationToken);
        Task<bool> IsUserEmailUnique(string emailAddress, CancellationToken cancellationToken);
    }
}
