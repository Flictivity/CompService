using CompService.Core.Enums;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IOrderService _orderService;

    public UserService(IUserRepository userRepository, IOrderService orderService)
    {
        _userRepository = userRepository;
        _orderService = orderService;
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

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(Roles role)
    {
        var users = await GetAllUsersAsync();
        return users.Where(x => x.Roles == role);
    }

    public async Task<IEnumerable<User>> GetFreeMastersAsync()
    {
        var res = new List<User>();
        var users = await GetUsersByRoleAsync(Roles.Master);
        foreach (var user in users)
        {
            if(await _orderService.GetMasterOrdersCount(user.UserId) < 3)
            {
                res.Add(user);
            }
        }

        return res;
    }

    public async Task<BaseResult> ChangeUserDataAsync(string name, string surname, string patronymic, string email,
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

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsers();
    }
}