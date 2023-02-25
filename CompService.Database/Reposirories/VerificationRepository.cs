using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;

namespace CompService.Database.Reposirories;

public class VerificationRepository : IVerificationRepository
{
    private readonly ApplicationContext _context;
    private readonly ILogger<VerificationRepository> _logger;

    public VerificationRepository(ApplicationContext context, ILogger<VerificationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task CreateVerification(UserVerification? verification)
    {
        if (verification is null)
        {
            _logger.LogError("Verification is null");
            return;
        }

        var verificationDb = new UserVerificationDb
        {
            IsActual = verification.IsActual,
            Code = verification.Code,
            UserId = verification.UserId,
            ExpyreTime = verification.ExpyreTime
        };
        _context.UserVerifications.Add(verificationDb);
        await _context.SaveChangesAsync();
    }

    public Task<UserVerification?> VerificateUser(string email)
    {
        var res = _context.UserVerifications.FirstOrDefault(x =>
            x.User.Email == email && x.IsActual);

        return Task.FromResult(res is null
            ? null
            : new UserVerification
            {
                Id = res.Id,
                Code = res.Code,
                IsActual = res.IsActual,
                UserId = res.UserId 
            });
    }

    public async Task ChangeVerification(int id)
    {
        var verification = _context.UserVerifications.FirstOrDefault(x => x.Id == id);
        if (verification != null) verification.IsActual = false;
        await _context.SaveChangesAsync();
    }
}