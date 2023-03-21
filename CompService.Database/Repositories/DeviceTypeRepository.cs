using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Repositories;

public class DeviceTypeRepository : IReferenceRepository<DeviceType>
{
    private readonly IMongoCollection<DeviceTypeDb> _defects;
    private readonly ILogger<DeviceTypeRepository> _logger;
    public DeviceTypeRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<DeviceTypeRepository> logger)
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
            : EntityConverter.ConvertDeviceType(res);
    }

    public async Task UpdateReference(DeviceType currentRef, DeviceType newRef)
    {
        var newDbRef = EntityConverter.ConvertDeviceType(newRef);

        await _defects.ReplaceOneAsync(x => x.DeviceTypeId == currentRef.DeviceTypeId, newDbRef);
    }

    public async Task<IEnumerable<DeviceType>> GetAllValues()
    {
        var defects = (await _defects.FindAsync(x => true)).ToList();

        return defects.Select(reference => 
            new DeviceType {DeviceTypeId = reference.DeviceTypeId, Name = reference.Name,}).ToList();
    }
}