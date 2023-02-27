namespace CompService.Core.Models
{
    public class User
    {
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Password { get; set; } 
        public string? PhoneNumber { get; set; }
    }
}
