using CompService.Core.Models;

namespace CompService.Core.Services;

public interface IUserService
{
    public Task CreateUserAsync(User user);
    public Task<User?> GetUserByEmailAsync(string? email);
}