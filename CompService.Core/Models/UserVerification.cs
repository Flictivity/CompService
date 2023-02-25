namespace CompService.Core.Models;

public class UserVerification
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public bool IsActual { get; set; } = true;
    public DateTime ExpyreTime { get; set; }
    public int UserId { get; set; }
}