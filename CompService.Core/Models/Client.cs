namespace CompService.Core.Models;

public class Client
{
    public string ClientId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Source? Source { get; set; }
}