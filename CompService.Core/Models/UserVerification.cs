namespace CompService.Core.Models;

public class UserVerification
{
    public int Id { get; set; }
    public string Code { get; set; }
    public bool IsActual { get; set; }
    public User User { get; set; }
}