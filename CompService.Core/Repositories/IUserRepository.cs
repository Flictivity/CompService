using CompService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompService.Core.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> CreateUser(User user);
        public Task<User?> GetUserByEmail(string? email);
    }
}
