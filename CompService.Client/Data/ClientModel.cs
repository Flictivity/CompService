using CompService.Core.Models;

namespace CompService.Client.Data;

public class ClientModel
{
    public string Name { get; set; } = String.Empty;
    public string Surname { get; set; } = String.Empty;
    public string? Email { get; set; } = String.Empty;
    public string? PhoneNumber { get; set; } = String.Empty;
    public Source? Source { get; set; } = null;
}