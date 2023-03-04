using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using CompService.Database.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Reposirories;

public class VerificationRepository : IVerificationRepository
{
    private readonly IMongoCollection<UserVerificationDb> _verifications;
    private readonly ILogger<VerificationRepository> _logger;

    public VerificationRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<VerificationRepository> logger)
    {
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _verifications = mongoDatabase.GetCollection<UserVerificationDb>(
            databaseConnectionSettings.Value.UserVerificationCollectionName);
        _logger = logger;
    }

    public async Task CreateVerification(UserVerification? verification)
    {
        if (verification is null)
        {
            _logger.LogError("Verification is null");
            return;
        }

        var verificateUser = new UserDb
        {
            UserId = verification.User.UserId,
            Name = verification.User.Name,
            Surname = verification.User.Surname,
            Patronymic = verification.User.Patronymic,
            Email = verification.User.Email,
            Password = verification.User.Password,
            PhoneNumber = verification.User.PhoneNumber
        };
        var verificationDb = new UserVerificationDb
        {
            IsActual = verification.IsActual,
            Code = verification.Code,
            User = verificateUser,
            ExpyreTime = verification.ExpyreTime
        };
        await _verifications.InsertOneAsync(verificationDb);
    }

    public async Task<UserVerification?> VerifyUser(string email)
    {
        var res = (await _verifications.FindAsync(x =>
            x.User.Email == email && x.IsActual)).FirstOrDefault();

        return res is null
            ? null
            : new UserVerification
            {
                VerificationId = res.VerificationId,
                Code = res.Code,
                IsActual = res.IsActual,
                User = new User
                {
                    UserId = res.User.UserId,
                    Name = res.User.Name,
                    Email = res.User.Email,
                    Surname = res.User.Surname,
                    Patronymic = res.User.Patronymic,
                    Password = res.User.Password,
                    PhoneNumber = res.User.PhoneNumber
                }
            };
    }

    public async Task ChangeVerification(string id)
    {
        var verification = (await _verifications
            .FindAsync(x => x.VerificationId == id)).FirstOrDefault();

        if (verification is null)
        {
            return;
        }

        verification.IsActual = false;
        await _verifications
            .ReplaceOneAsync(x => x.VerificationId == verification.VerificationId, verification);
    }
}