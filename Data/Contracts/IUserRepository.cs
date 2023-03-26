using Entities;
using System.Threading;
using System.Threading.Tasks;
using Data.Contracts;
using Entities.UserManager;

namespace Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUserAndPass(string username, string password, CancellationToken cancellationToken);
        Task UpdateSecuirtyStampAsync(User user, CancellationToken cancellationToken);
        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
        Task AddAsync(User user, string password, CancellationToken cancellationToken);
    }
}