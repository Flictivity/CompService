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
    private readonly IReferenceRepository<Source> _sourceRepository;

    public ClientRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<ClientRepository> logger, IReferenceRepository<Source> sourceRepository)
    {
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _clients = mongoDatabase.GetCollection<ClientDb>(
            databaseConnectionSettings.Value.ClientsCollectionName);
        _logger = logger;
        _sourceRepository = sourceRepository;
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
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    public async Task<Client?> GetClientByEmail(string? email)
    {
        var res = (await _clients.FindAsync(x => x.Email == email)).FirstOrDefault();
        if (res is null)
        {
            return null;
        }

        var source = await _sourceRepository.GetReferenceById(res.SourceId);
        var client = EntityConverter.ConvertClient(res);
        client.Source = source;

        return client;
    }

    public async Task<Client?> GetClientById(string? id)
    {
        var res = (await _clients.FindAsync(x => x.ClientId == id)).FirstOrDefault();
        if (res is null)
        {
            return null;
        }

        var source = await _sourceRepository.GetReferenceById(res.SourceId);
        var client = EntityConverter.ConvertClient(res);
        client.Source = source;

        return client;
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
            SourceId = newClient.Source?.SourceId
        };

        await _clients.ReplaceOneAsync(x => x.ClientId == currentClient.ClientId, newDbUser);
    }

    public async Task<IEnumerable<Client>> GetAllClients()
    {
        var res = new List<Client>();
        var users = (await _clients.FindAsync(x => true)).ToList();
        foreach (var user in users)
        {
            var source = await _sourceRepository.GetReferenceById(user.SourceId);
            var client = EntityConverter.ConvertClient(user);
            client.Source = source;
            res.Add(client);
        }

        return res;
    }
}