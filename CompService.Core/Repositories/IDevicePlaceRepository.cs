using CompService.Core.Models;

namespace CompService.Core.Repositories;

public interface IDevicePlaceRepository
{
    public Task Create(DevicePlace devicePlace);
    public Task<DevicePlace?> GetPlaceById(string id);
    public Task<DevicePlace?> GetPlaceByOrder(string orderId);
    public Task UpdateDevicePlace(DevicePlace currentPlace, DevicePlace newPlace);
    public Task<IEnumerable<DevicePlace>> GetAllPlaces();
}