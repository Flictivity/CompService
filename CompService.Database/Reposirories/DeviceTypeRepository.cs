using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using CompService.Database.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Reposirories;

public class DeviceTypeRepository : IReferenceRepository<DeviceType>
{
    private readonly IMongoCollection<DeviceTypeDb> _defects;
    private readonly ILogger<IReferenceRepository<DeviceType>> _logger;
    public DeviceTypeRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<IReferenceRepository<DeviceType>> logger)
    {
        _logger = logger;
        
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _defects = mongoDatabase.GetCollection<DeviceTypeDb>(
            databaseConnectionSettings.Value.DeviceTypesCollectionName);
    }
    
    public async Task Create(DeviceType newRef)
    {
        var dbRef = new DeviceTypeDb
        {
            Name = newRef.Name
        };
        await _defects.InsertOneAsync(dbRef);
    }

    public async Task<DeviceType?> GetReferenceById(string? id)
    {
        var res = (await _defects.FindAsync(x => x.DeviceTypeId == id)).FirstOrDefault();

        return res is null
            ? null
            : new DeviceType
            {
                DeviceTypeId = res.DeviceTypeId,
                Name = res.Name
            };
    }

    public async Task UpdateReference(DeviceType? currentRef, DeviceType newRef)
    {
        if (currentRef is null)
        {
            _logger.LogError("Null reference");
            return;
        }

        var newDbRef = new DeviceTypeDb()
        {
            DeviceTypeId = newRef.DeviceTypeId,
            Name = newRef.Name
        };

        await _defects.ReplaceOneAsync(x => x.DeviceTypeId == currentRef.DeviceTypeId, newDbRef);
    }

    public async Task<IEnumerable<DeviceType>> GetAllValues()
    {
        var defects = (await _defects.FindAsync(x => true)).ToList();
        var res = new List<DeviceType>();
        foreach (var reference in defects)
        {
            res.Add(new DeviceType()
            {
                DeviceTypeId = reference.DeviceTypeId,
                Name = reference.Name,
            });
        }

        return res;
    }
}