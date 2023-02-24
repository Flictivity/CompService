using System.Data;

namespace CompService.Client.Data
{
    public class LoginCredentials
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
