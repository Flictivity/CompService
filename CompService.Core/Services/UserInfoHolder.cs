using CompService.Core.Models;

namespace CompService.Core.Services;

public class UserInfoHolder
{
    public User? CurrentUser { get; set; } = null!;
}