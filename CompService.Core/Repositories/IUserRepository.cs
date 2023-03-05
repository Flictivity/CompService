using CompService.Core.Models;

namespace CompService.Core.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> CreateUser(User user);
        public Task<User?> GetUserByEmail(string? email);
        public Task<User?> GetUserById(string? id);
        public Task UpdateUser(User? currentUser, User newUser);
    }
}
