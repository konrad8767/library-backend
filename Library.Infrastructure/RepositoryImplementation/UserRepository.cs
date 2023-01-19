using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Infrastructure.RepositoryImplementation
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _dbContext;

        public UserRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateUser(User createdUser, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(createdUser, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return createdUser.Id;
        }

        public async Task<User> GetUserByLogin(string login, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Login == login, cancellationToken);
        }

        public async Task<User> GetUserById(int userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        }

        public async Task<bool> IsUserLoginUnique(string login, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Users
                .Where(x => x.Login == login)
                .CountAsync(cancellationToken);

            if (result > 0)
                return false;

            return true;
        }

        public async Task<bool> IsUserEmailUnique(string emailAddress, CancellationToken cancellationToken)
        {
            var emailExists = await _dbContext.Users.AnyAsync(x => x.EmailAddress.ToLower() == emailAddress.ToLower(), cancellationToken);

            if (emailExists)
                return false;

            return true;
        }

        public async Task<bool> IsUserInDb(int userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.AnyAsync(x => x.Id == userId, cancellationToken);
        }

        public async Task UpdateUser(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
