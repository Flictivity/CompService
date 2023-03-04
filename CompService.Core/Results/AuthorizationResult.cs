using CompService.Core.Models;

namespace CompService.Core.Results;

public class AuthorizationResult : BaseResult
{
    public User? User { get; set; }
}