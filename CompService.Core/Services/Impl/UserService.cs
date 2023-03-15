using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

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

    public async Task<User?> GetUserByIdAsync(string? id)
    {
        return await _userRepository.GetUserById(id);
    }

    public async Task<BaseResult> ChangeUserData(string name, string surname, string patronymic, string email,
        string password, string phoneNumber, User changeUser)
    {
        var newUser = new User
        {
            Name = name,
            Surname = surname,
            Patronymic = patronymic,
            Email = email,
            Password = password,
            PhoneNumber = phoneNumber
        };
        await _userRepository.UpdateUser(changeUser, newUser);

        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }
}