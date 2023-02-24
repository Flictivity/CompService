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

    public async Task CreateVerification(User user)
    {
        var userDb = new UserDb
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            PhoneNumber = user.PhoneNumber
        };
        var rnd = new Random();
        var verificationDb = new UserVerificationDb
        {
            IsActual = true,
            Code = rnd.Next(0, 1000000).ToString("D6"),
            User = userDb
        };
        _context.UserVerifications.Add(verificationDb);
        _context.SaveChangesAsync();
    }

    public Task<bool> VerificateUser(string email, string code)
    {
        var res = _context.UserVerifications.FirstOrDefault(x =>
            x.User.Email == email);

        if (!Equals(res.Code, code))
        {
            return Task.FromResult(false);
        }

        if (!res.IsActual)
        {
            return Task.FromResult(false);
        }

        res.IsActual = false;
        _context.SaveChanges();
        return Task.FromResult(true);
    }
}