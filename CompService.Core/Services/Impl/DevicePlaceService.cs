using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class DevicePlaceService : IDevicePlaceService
{
    private readonly IDevicePlaceRepository _devicePlaceRepository;

    public DevicePlaceService(IDevicePlaceRepository devicePlaceRepository)
    {
        _devicePlaceRepository = devicePlaceRepository;
    }

    public async Task<BaseResult> CreateAsync(DevicePlace place)
    {
        await _devicePlaceRepository.Create(place);
        return new BaseResult {Success = true};
    }

    public async Task<DevicePlace?> GetPlaceByIdAsync(string id)
    {
        return await _devicePlaceRepository.GetPlaceById(id);
    }

    public async Task<BaseResult> UpdateDevicePlaceAsync(DevicePlace currentPlace, DevicePlace newPlace)
    {
        await _devicePlaceRepository.UpdateDevicePlace(currentPlace, newPlace);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<DevicePlace>> GetAllPlacesAsync()
    {
        return await _devicePlaceRepository.GetAllPlaces();
    }

    public async Task<IEnumerable<DevicePlace>> GetFreePlacesAsync()
    {
        return await _devicePlaceRepository.GetFreePlaces();
    }
}