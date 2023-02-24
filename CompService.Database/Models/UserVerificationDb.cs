using CompService.Core.Models;

namespace CompService.Database.Models;

public class UserVerificationDb
{
    public int Id { get; set; }
    public string Code { get; set; }
    public bool IsActual { get; set; }
    public UserDb User { get; set; }
}