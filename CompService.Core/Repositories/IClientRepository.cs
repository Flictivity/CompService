using CompService.Core.Models;

namespace CompService.Core.Repositories;

public interface IClientRepository
{
    public Task<bool> CreateClient(Client client);
    public Task<Client?> GetClientByEmail(string? email);
    public Task<Client?> GetClientById(string? id);
    public Task UpdateClient(Client currentClient, Client newClient);
    public Task<IEnumerable<Client>> GetAllClients();
}