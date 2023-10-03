using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Repositories;

public class FacilityRepository : IFacilityRepository
{
    private readonly IMongoCollection<FacilityDb> _facilities;
    private readonly ILogger<FacilityRepository> _logger;

    public FacilityRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<FacilityRepository> logger)
    {
        _logger = logger;

        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _facilities = mongoDatabase.GetCollection<FacilityDb>(
            databaseConnectionSettings.Value.FacilitiesCollectionName);
    }

    public async Task CreateFacility(Facility facility)
    {
        var dbRef = EntityConverter.ConvertFacility(facility);
        await _facilities.InsertOneAsync(dbRef);
    }

    public async Task<Facility?> GetFacilityById(string? id)
    {
        var res = (await _facilities.FindAsync(x => x.FacilityId == id)).FirstOrDefault();

        return res is null
            ? null
            : EntityConverter.ConvertFacility(res);
    }

    public async Task UpdateFacility(Facility currentFacility, Facility newFacility)
    {
        var newDbFacility = EntityConverter.ConvertFacility(newFacility);

        await _facilities.ReplaceOneAsync(x => x.FacilityId == currentFacility.FacilityId, newDbFacility);
    }

    public async Task<IEnumerable<Facility>> GetAllFacilities()
    {
        var facilities = (await _facilities.FindAsync(x => true)).ToList();

        return facilities.Select(EntityConverter.ConvertFacility).ToList();
    }
}