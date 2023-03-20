using CompService.Core.Enums;
using CompService.Core.Results;

namespace CompService.Core.Services
{
    public interface IAuthorizationService
    {
        public Task<BaseResult> RegistrateAsync(string name, string surname, string patronymic,
            string email, string? phoneNumber, Roles roles);
        public Task<BaseResult> CreateAuthorizeCodeAsync(string email);
        public Task<AuthorizationResult> AuthorizeWithCodeAsync(string email, string code);
        public Task<AuthorizationResult> AuthorizeWithPassword(string email, string password);
    }
}
