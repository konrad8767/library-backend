using Library.Domain.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace Library.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CreateUser(User createdUser, CancellationToken cancellationToken);
        Task UpdateUser(User user, CancellationToken cancellationToken);
        Task UpdateUsers(List<User> users, CancellationToken cancellationToken);
        Task<User> GetUserByLogin(string login, CancellationToken cancellationToken);
        Task<bool> IsUserLoginUnique(string login, CancellationToken cancellationToken);
        Task<bool> IsUserEmailUnique(string emailAddress, CancellationToken cancellationToken);
        Task<bool> IsUserInDb(int userId, CancellationToken cancellationToken);
        Task<User> GetUserById(int userId, CancellationToken cancellationToken);
        Task<List<User>> GetUsersByIds(List<int> userIds, CancellationToken cancellationToken);
        Task<List<User>> GetSpectatorsByBookId(int bookId, CancellationToken cancellationToken);
    }
}
