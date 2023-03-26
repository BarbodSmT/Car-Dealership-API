using Entities;
using System.Threading.Tasks;
using Entities.UserManager;

namespace Services
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
    }
}