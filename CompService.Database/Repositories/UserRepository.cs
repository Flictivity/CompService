using CompService.Core.Enums;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserDb> _users;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
            ILogger<UserRepository> logger)
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
                var userDb = EntityConverter.ConvertUser(user);
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
            
            return res is null ? null : EntityConverter.ConvertUser(res); 
        }

        public async Task<User?> GetUserById(string? id)
        {
            var res = (await _users.FindAsync(x => x.UserId == id)).FirstOrDefault();
            
            return res is null ? null : EntityConverter.ConvertUser(res); 
        }

        public async Task UpdateUser(User currentUser, User newUser)
        {
            var newDbUser = new UserDb
            {
                UserId = currentUser.UserId,
                Name = newUser.Name,
                Surname = newUser.Surname,
                Patronymic = newUser.Patronymic,
                Email = newUser.Email,
                Password = newUser.Password,
                PhoneNumber = newUser.PhoneNumber,
                Role = (int)currentUser.Roles
            };
            
            await _users.ReplaceOneAsync(x => x.UserId == currentUser.UserId, newDbUser);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = (await _users.FindAsync(x => true)).ToList();

            return users.Select(EntityConverter.ConvertUser).ToList();
        }
    }
}
