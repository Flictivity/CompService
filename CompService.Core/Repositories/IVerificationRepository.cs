using CompService.Core.Models;

namespace CompService.Core.Repositories;

public interface IVerificationRepository
{
    public Task CreateVerification(User? user);
    public Task<bool> VerificateUser(string email, string code);
}