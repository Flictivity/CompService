using CompService.Core.Enums;

namespace CompService.Client.Data;

public class UserModel
{
    public string Name { get; set; } = String.Empty;
    public string Surname { get; set; } = String.Empty;
    public string Patronymic { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string? Password { get; set; } = String.Empty;
    public string? PhoneNumber { get; set; } = String.Empty;
    public Roles Roles { get; set; }
}