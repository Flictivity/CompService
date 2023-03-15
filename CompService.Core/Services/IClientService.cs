using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services;

public interface IClientService
{
    public Task<BaseResult> CreateClientAsync(Client client);
    public Task<Client?> GetClientByEmailAsync(string? email);
    public Task<Client?> GetClientByIdAsync(string? id);

    public Task<BaseResult> ChangeClientData(string name, string surname, string? email,
         string? phoneNumber, Client changeClient, Source? source);

    public Task<IEnumerable<Client>> GetAllClients();
}