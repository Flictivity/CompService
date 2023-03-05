using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserInfoHolder _userInfoHolder;

    public UserService(IUserRepository userRepository, UserInfoHolder userInfoHolder)
    {
        _userRepository = userRepository;
        _userInfoHolder = userInfoHolder;
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
        string password, string phoneNumber)
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
        await _userRepository.UpdateUser(_userInfoHolder.CurrentUser, newUser);

        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }
}