using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompService.Database.Reposirories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<IUserRepository> _logger;

        public UserRepository(ApplicationContext context, ILogger<IUserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<bool> CreateUser(User user)
        {
            try
            {
                var userDb = new UserDb
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber,
                };
                _context.Users.Add(userDb);
                _context.SaveChanges();
                
                return Task.FromResult(true);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Task.FromResult(false);
            }
        }

        public Task<User?> GetUserByEmail(string? email)
        {
            var res = _context.Users.FirstOrDefault(x => x.Email == email);
            
            return Task.FromResult(res is null ? null : new User
            {
                UserId = res.UserId,
                Email = res.Email,
                PhoneNumber = res.PhoneNumber,
                Password = res.Password,
                Name = res.Name
            }); 
        }
    }
}
