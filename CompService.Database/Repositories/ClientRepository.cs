using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly IMongoCollection<ClientDb> _clients;
    private readonly ILogger<ClientRepository> _logger;

    public ClientRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<ClientRepository> logger)
    {
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _clients = mongoDatabase.GetCollection<ClientDb>(
            databaseConnectionSettings.Value.ClientsCollectionName);
        _logger = logger;
    }
    
    public async Task<bool> CreateClient(Client client)
    {
        try
        {
            if (client.Source is null)
            {
                return false;
            }

            var clientDb = EntityConverter.ConvertClient(client);
            await _clients.InsertOneAsync(clientDb);

            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    public async Task<Client?> GetClientByEmail(string? email)
    {
        var res = (await _clients.FindAsync(x => x.Email == email)).FirstOrDefault();
        return res is null ? null : EntityConverter.ConvertClient(res);
    }

    public async Task<Client?> GetClientById(string? id)
    {
        var res = (await _clients.FindAsync(x => x.ClientId == id)).FirstOrDefault();

        return res is null ? null : EntityConverter.ConvertClient(res);
    }

    public async Task UpdateClient(Client currentClient, Client newClient)
    {
        var newDbUser = new ClientDb
        {
            ClientId = currentClient.ClientId,
            Name = newClient.Name,
            Surname = newClient.Surname,
            Email = newClient.Email,
            PhoneNumber = newClient.PhoneNumber,
            Source = new SourceDb
            {
                SourceId = newClient.Source!.SourceId,
                Name = newClient.Source.Name
            }
        };
            
        await _clients.ReplaceOneAsync(x => x.ClientId == currentClient.ClientId, newDbUser);
    }

    public async Task<IEnumerable<Client>> GetAllClients()
    {
        var users = (await _clients.FindAsync(x => true)).ToList();

        return users.Select(EntityConverter.ConvertClient).ToList();
    }
}