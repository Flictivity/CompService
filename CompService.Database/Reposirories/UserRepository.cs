using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using CompService.Database.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Reposirories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserDb> _users;
        private readonly ILogger<IUserRepository> _logger;

        public UserRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
            ILogger<IUserRepository> logger)
        {
            var mongoClient = new MongoClient(
                databaseConnectionSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseConnectionSettings.Value.DatabaseName);

            _users = mongoDatabase.GetCollection<UserDb>(
                databaseConnectionSettings.Value.UsersCollectionName);
            _logger = logger;
        }

        public async Task<bool> CreateUser(User user)
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
                await _users.InsertOneAsync(userDb);

                return await Task.FromResult(true);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return await Task.FromResult(false);
            }
        }

        public Task<User?> GetUserByEmail(string? email)
        {
            var res = _users.Find(x => x.Email == email).FirstOrDefault();
            
            return Task.FromResult(res is null ? null : new User
            {
                UserId = res.UserId,
                Email = res.Email,
                PhoneNumber = res.PhoneNumber,
                Name = res.Name
            }); 
        }
    }
}
