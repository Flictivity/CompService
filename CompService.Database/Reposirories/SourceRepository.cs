using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using CompService.Database.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Reposirories;

public class SourceRepository : IReferenceRepository<Source>
{
    private readonly IMongoCollection<SourceDb> _sources;
    private readonly ILogger<IReferenceRepository<Source>> _logger;
    
    public SourceRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<IReferenceRepository<Source>> logger)
    {
        _logger = logger;
        
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _sources = mongoDatabase.GetCollection<SourceDb>(
            databaseConnectionSettings.Value.SourcesCollectionName);
    }

    public async Task Create(Source newRef)
    {
        var dbRef = new SourceDb
        {
            Name = newRef.Name
        };
        await _sources.InsertOneAsync(dbRef);
    }

    public async Task<Source?> GetReferenceById(string? id)
    {
        var res = (await _sources.FindAsync(x => x.SourceId == id)).FirstOrDefault();

        return res is null
            ? null
            : new Source
            {
                SourceId = res.SourceId,
                Name = res.Name
            };
    }

    public async Task UpdateReference(Source? currentRef, Source newRef)
    {
        if (currentRef is null)
        {
            _logger.LogError("Null reference");
            return;
        }

        var newDbRef = new SourceDb
        {
            SourceId = newRef.SourceId,
            Name = newRef.Name
        };

        await _sources.ReplaceOneAsync(x => x.SourceId == currentRef.SourceId, newDbRef);
    }

    public async Task<IEnumerable<Source>> GetAllValues()
    {
        var sources = (await _sources.FindAsync(x => true)).ToList();
        var res = new List<Source>();
        foreach (var reference in sources)
        {
            res.Add(new Source
            {
                SourceId = reference.SourceId,
                Name = reference.Name,
            });
        }

        return res;
    }
}