using CompService.Core.Messages;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    public async Task<BaseResult> CreateClientAsync(Client client)
    {
        var dbClient = await GetClientByEmailAsync(client.Email);

        if (dbClient is not null)
        {
            return new BaseResult{Success = false, Message = UserMessages.UserAlreadyExisting};
        }
            
       await _clientRepository.CreateClient(client);
       
       return new BaseResult{Success = true};
    }

    public async Task<Client?> GetClientByEmailAsync(string? email)
    {
        return await _clientRepository.GetClientByEmail(email);
    }

    public async Task<Client?> GetClientByIdAsync(string? id)
    {
        return await _clientRepository.GetClientById(id);
    }

    public async Task<BaseResult> ChangeClientData(string name, string surname, string? email, string? phoneNumber,
        Client changeClient, Source? source)
    {
        var newClient = new Client
        {
            ClientId = changeClient.ClientId,
            Name = name,
            Surname = surname,
            Email = email,
            PhoneNumber = phoneNumber,
            Source = source
        };
        await _clientRepository.UpdateClient(changeClient, newClient);

        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<Client>> GetAllClients()
    {
        return await _clientRepository.GetAllClients();
    }
}