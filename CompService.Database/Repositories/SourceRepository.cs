using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Repositories;

public class SourceRepository : IReferenceRepository<Source>
{
    private readonly IMongoCollection<SourceDb> _sources;
    private readonly ILogger<SourceRepository> _logger;
    
    public SourceRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<SourceRepository> logger)
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
            : EntityConverter.ConvertSource(res);
    }

    public async Task UpdateReference(Source currentRef, Source newRef)
    {
        var newDbRef = EntityConverter.ConvertSource(newRef);

        await _sources.ReplaceOneAsync(x => x.SourceId == currentRef.SourceId, newDbRef);
    }

    public async Task<IEnumerable<Source>> GetAllValues()
    {
        var sources = (await _sources.FindAsync(x => true)).ToList();

        return sources.Select(reference => 
            new Source {SourceId = reference.SourceId, Name = reference.Name,}).ToList();
    }
}