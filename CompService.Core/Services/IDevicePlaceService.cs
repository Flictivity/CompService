using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services;

public interface IDevicePlaceService
{
    public Task<BaseResult> CreateAsync(DevicePlace place);
    public Task<DevicePlace?> GetPlaceByIdAsync(string id);

    public Task<BaseResult> UpdateDevicePlaceAsync(DevicePlace currentPlace, DevicePlace newPlace);

    public Task<IEnumerable<DevicePlace>> GetAllPlacesAsync();
    public Task<IEnumerable<DevicePlace>> GetFreePlacesAsync();
}