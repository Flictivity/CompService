namespace CompService.Core.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Password { get; set; } 
        public string? PhoneNumber { get; set; }
    }
}
