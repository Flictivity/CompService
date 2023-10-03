using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Repositories;

public class VerificationRepository : IVerificationRepository
{
    private readonly IMongoCollection<UserVerificationDb> _verifications;
    private readonly ILogger<VerificationRepository> _logger;
    private readonly IUserRepository _userRepository;

    public VerificationRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<VerificationRepository> logger, IUserRepository userRepository)
    {
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _verifications = mongoDatabase.GetCollection<UserVerificationDb>(
            databaseConnectionSettings.Value.UserVerificationCollectionName);
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task CreateVerification(UserVerification verification)
    {
        if (verification.User is null)
        {
            _logger.LogError("User is null");
            return;
        }

        var verificationUser = EntityConverter.ConvertUser(verification.User);
        
        var verificationDb = new UserVerificationDb
        {
            IsActual = verification.IsActual,
            Code = verification.Code,
            UserId = verificationUser.UserId,
            ExpyreTime = verification.ExpyreTime,
            UserEmail = verificationUser.Email
        };
        await _verifications.InsertOneAsync(verificationDb);
    }

    public async Task<UserVerification?> VerifyUser(string email)
    {
        var res = (await _verifications.FindAsync(x =>
            x.UserEmail == email && x.IsActual)).FirstOrDefault();

        return res is null
            ? null
            : new UserVerification
            {
                VerificationId = res.VerificationId,
                Code = res.Code,
                IsActual = res.IsActual,
                User = await _userRepository.GetUserById(res.UserId)
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