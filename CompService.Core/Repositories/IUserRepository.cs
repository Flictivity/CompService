using CompService.Core.Models;

namespace CompService.Core.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> CreateUser(User user);
        public Task<User?> GetUserByEmail(string? email);
    }
}
