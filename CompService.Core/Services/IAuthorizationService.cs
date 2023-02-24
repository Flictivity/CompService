using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompService.Core.Models;

namespace CompService.Core.Services
{
    public interface IAuthorizationService
    {
        public Task<bool> RegistrateAsync(string name, string email, string? phoneNumber);
        public Task<bool> CreateAuthorizeCodeAsync(string email);
        public Task<User?> AuthorizeAsync(string email, string code);
    }
}
