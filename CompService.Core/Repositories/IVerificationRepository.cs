using CompService.Core.Models;

namespace CompService.Core.Repositories;

public interface IVerificationRepository
{
    public Task CreateVerification(UserVerification? verification);
    public Task<UserVerification?> VerificateUser(string email);
    public Task ChangeVerification(int id);
}