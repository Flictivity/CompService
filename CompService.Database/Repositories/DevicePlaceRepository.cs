using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Repositories;

public class DevicePlaceRepository : IDevicePlaceRepository
{
    private readonly IMongoCollection<DevicePlaceDb> _places;
    private readonly ILogger<DevicePlaceRepository> _logger;

    public DevicePlaceRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings
        , ILogger<DevicePlaceRepository> logger)
    {
        var mongoClient = new MongoClient(databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(databaseConnectionSettings.Value.DatabaseName);

        _places = mongoDatabase.GetCollection<DevicePlaceDb>(
            databaseConnectionSettings.Value.DevicePlacesCollectionName);
        _logger = logger;
    }

    public async Task Create(DevicePlace devicePlace)
    {
        await _places.InsertOneAsync(EntityConverter.ConvertDevicePlace(devicePlace));
    }

    public async Task<DevicePlace?> GetPlaceById(string id)
    {
        var res = (await _places.FindAsync(x => x.PlaceId == id))
            .FirstOrDefault();
        return res is null
            ? null
            : EntityConverter.ConvertDevicePlace(res);
    }

    public async Task UpdateDevicePlace(DevicePlace currentPlace, DevicePlace newPlace)
    {
        var newDbPlace = new DevicePlaceDb
        {
            PlaceId = currentPlace.PlaceId,
            Info = newPlace.Info,
            IsOccupied = newPlace.IsOccupied
        };

        await _places.ReplaceOneAsync(x => x.PlaceId == currentPlace.PlaceId, newDbPlace);
    }

    public async Task<IEnumerable<DevicePlace>> GetAllPlaces()
    {
        var places = (await _places.FindAsync(x => true)).ToList();
        return places.Select(EntityConverter.ConvertDevicePlace);
    }
}