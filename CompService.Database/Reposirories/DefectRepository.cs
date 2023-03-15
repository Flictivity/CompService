using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using CompService.Database.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Reposirories;

public class DefectRepository : IReferenceRepository<Defect>
{
    private readonly IMongoCollection<DefectDb> _defects;
    private readonly ILogger<IReferenceRepository<Defect>> _logger;
    public DefectRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<IReferenceRepository<Defect>> logger)
    {
        _logger = logger;
        
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _defects = mongoDatabase.GetCollection<DefectDb>(
            databaseConnectionSettings.Value.DefectsCollectionName);
    }
    
    public async Task Create(Defect newRef)
    {
        var dbRef = new DefectDb
        {
            Name = newRef.Name
        };
        await _defects.InsertOneAsync(dbRef);
    }

    public async Task<Defect?> GetReferenceById(string? id)
    {
        var res = (await _defects.FindAsync(x => x.DefectId == id)).FirstOrDefault();

        return res is null
            ? null
            : new Defect
            {
                DefectId = res.DefectId,
                Name = res.Name
            };
    }

    public async Task UpdateReference(Defect? currentRef, Defect newRef)
    {
        if (currentRef is null)
        {
            _logger.LogError("Null reference");
            return;
        }

        var newDbRef = new DefectDb
        {
            DefectId = newRef.DefectId,
            Name = newRef.Name
        };

        await _defects.ReplaceOneAsync(x => x.DefectId == currentRef.DefectId, newDbRef);
    }

    public async Task<IEnumerable<Defect>> GetAllValues()
    {
        var defects = (await _defects.FindAsync(x => true)).ToList();
        var res = new List<Defect>();
        foreach (var reference in defects)
        {
            res.Add(new Defect
            {
                DefectId = reference.DefectId,
                Name = reference.Name,
            });
        }

        return res;
    }
}