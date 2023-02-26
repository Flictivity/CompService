using System.ComponentModel.DataAnnotations;

namespace CompService.Database.Models
{
    public class UserDb
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public List<UserVerificationDb> Verification { get; set; } = null!;
    }
}
