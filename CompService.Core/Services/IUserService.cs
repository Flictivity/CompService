using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services;

public interface IUserService
{
    public Task CreateUserAsync(User user);
    public Task<User?> GetUserByEmailAsync(string? email);
    public Task<User?> GetUserByIdAsync(string? id);

    public Task<BaseResult> ChangeUserDataAsync(string name, string surname, string patronymic, string email,
        string password, string phoneNumber, User changeUser);

    public Task<IEnumerable<User>> GetAllUsersAsync();
}