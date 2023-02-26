namespace CompService.Database.Models;

public class UserVerificationDb
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public bool IsActual { get; set; } = true;
    public DateTime ExpyreTime { get; set; }
    public int UserId { get; set; }
    public UserDb User { get; set; } = null!;
}