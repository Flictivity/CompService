using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using CompService.Database.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Reposirories;

public class ClientRepository : IClientRepository
{
    private readonly IMongoCollection<ClientDb> _clients;
    private readonly ILogger<IClientRepository> _logger;

    public ClientRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<IClientRepository> logger)
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
            var clientDb = new ClientDb
            {
                Name = client.Name,
                Email = client.Email,
                Surname = client.Surname,
                PhoneNumber = client.PhoneNumber,
                Source = client.Source
            };
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
            
        return res is null ? null : new Client
        {
            ClientId = res.ClientId,
            Email = res.Email,
            Surname = res.Surname,
            PhoneNumber = res.PhoneNumber,
            Name = res.Name,
            Source = res.Source
        }; 
    }

    public async Task<Client?> GetClientById(string? id)
    {
        var res = (await _clients.FindAsync(x => x.ClientId == id)).FirstOrDefault();
            
        return res is null ? null : new Client
        {
            ClientId = res.ClientId,
            Email = res.Email,
            Surname = res.Surname,
            PhoneNumber = res.PhoneNumber,
            Name = res.Name,
            Source = res.Source
        }; 
    }

    public async Task UpdateClient(Client? currentClient, Client newClient)
    {
        if (currentClient is null)
        {
            return;
        }

        var newDbUser = new ClientDb
        {
            ClientId = newClient.ClientId,
            Name = newClient.Name,
            Surname = newClient.Surname,
            Email = newClient.Email,
            PhoneNumber = newClient.PhoneNumber,
            Source = newClient.Source
        };
            
        await _clients.ReplaceOneAsync(x => x.ClientId == currentClient.ClientId, newDbUser);
    }

    public async Task<IEnumerable<Client>> GetAllClients()
    {
        var users = (await _clients.FindAsync(x => true)).ToList();
        var res = new List<Client>();
        foreach (var client in users)
        {
            res.Add(new Client
            {
                ClientId = client.ClientId,
                Name = client.Name,
                Surname = client.Surname,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Source = client.Source,
            });
        }

        return res;
    }
}