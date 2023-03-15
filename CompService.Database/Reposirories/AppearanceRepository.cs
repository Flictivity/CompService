using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using CompService.Database.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Reposirories;

public class AppearanceRepository : IReferenceRepository<Appearance>
{
    private readonly IMongoCollection<AppearanceDb> _defects;
    private readonly ILogger<IReferenceRepository<Appearance>> _logger;
    public AppearanceRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<IReferenceRepository<Appearance>> logger)
    {
        _logger = logger;
        
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _defects = mongoDatabase.GetCollection<AppearanceDb>(
            databaseConnectionSettings.Value.AppearancesCollectionName);
    }

    public async Task Create(Appearance newRef)
    {
        var dbRef = new AppearanceDb()
        {
            Name = newRef.Name
        };
        await _defects.InsertOneAsync(dbRef);
    }

    public async Task<Appearance?> GetReferenceById(string? id)
    {
        var res = (await _defects.FindAsync(x => x.AppearanceId == id)).FirstOrDefault();

        return res is null
            ? null
            : new Appearance
            {
                AppearanceId = res.AppearanceId,
                Name = res.Name
            };
    }

    public async Task UpdateReference(Appearance? currentRef, Appearance newRef)
    {
        if (currentRef is null)
        {
            _logger.LogError("Null reference");
            return;
        }

        var newDbRef = new AppearanceDb
        {
            AppearanceId = newRef.AppearanceId,
            Name = newRef.Name
        };

        await _defects.ReplaceOneAsync(x => x.AppearanceId == currentRef.AppearanceId, newDbRef);
    }

    public async Task<IEnumerable<Appearance>> GetAllValues()
    {
        var defects = (await _defects.FindAsync(x => true)).ToList();
        var res = new List<Appearance>();
        foreach (var reference in defects)
        {
            res.Add(new Appearance
            {
                AppearanceId = reference.AppearanceId,
                Name = reference.Name,
            });
        }

        return res;
    }
}