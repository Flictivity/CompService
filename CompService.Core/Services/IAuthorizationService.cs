using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services
{
    public interface IAuthorizationService
    {
        public Task<BaseResult> RegistrateAsync(string name, string email, string? phoneNumber);
        public Task<BaseResult> CreateAuthorizeCodeAsync(string email);
        public Task<User?> AuthorizeAsync(string email, string code);
    }
}
