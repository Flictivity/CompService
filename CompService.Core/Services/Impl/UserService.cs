using CompService.Core.Models;
using CompService.Core.Repositories;

namespace CompService.Core.Services.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateUserAsync(User user)
    {
        await _userRepository.CreateUser(user);
    }
    
    public async Task<User?> GetUserByEmailAsync(string? email)
    {
        return await _userRepository.GetUserByEmail(email);
    }
}