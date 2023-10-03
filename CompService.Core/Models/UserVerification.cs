namespace CompService.Core.Models;

public class UserVerification
{
    public string VerificationId { get; set; } = null!;
    public string Code { get; set; } = null!;
    public bool IsActual { get; set; } = true;
    public DateTime ExpyreTime { get; set; }
    public User? User { get; set; } = null!;
}