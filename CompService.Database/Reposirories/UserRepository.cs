using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using CompService.Database.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
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
                    Patronymic = user.Patronymic,
                    Surname = user.Surname,
                    PhoneNumber = user.PhoneNumber,
                };
                await _users.InsertOneAsync(userDb);

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<User?> GetUserByEmail(string? email)
        {
            var res = (await _users.FindAsync(x => x.Email == email)).FirstOrDefault();
            
            return res is null ? null : new User
            {
                UserId = res.UserId,
                Email = res.Email,
                Surname = res.Surname,
                Patronymic = res.Patronymic,
                PhoneNumber = res.PhoneNumber,
                Password = res.Password,
                Name = res.Name
            }; 
        }

        public async Task<User?> GetUserById(string? id)
        {
            var res = (await _users.FindAsync(x => x.UserId == id)).FirstOrDefault();
            
            return res is null ? null : new User
            {
                UserId = res.UserId,
                Email = res.Email,
                Surname = res.Surname,
                Patronymic = res.Patronymic,
                PhoneNumber = res.PhoneNumber,
                Password = res.Password,
                Name = res.Name
            }; 
        }

        public async Task UpdateUser(User? currentUser, User newUser)
        {
            if (currentUser is null)
            {
                return;
            }

            var newDbUser = new UserDb
            {
                UserId = currentUser.UserId,
                Name = newUser.Name,
                Surname = newUser.Surname,
                Patronymic = newUser.Patronymic,
                Email = newUser.Email,
                Password = newUser.Password,
                PhoneNumber = newUser.PhoneNumber
            };
            
            await _users.ReplaceOneAsync(x => x.UserId == currentUser.UserId, newDbUser);
        }
    }
}
