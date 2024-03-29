﻿using CompService.Core.Models;

namespace CompService.Core.Repositories;

public interface IDevicePlaceRepository
{
    public Task Create(DevicePlace devicePlace);
    public Task<DevicePlace?> GetPlaceById(string id);
    public Task UpdateDevicePlace(DevicePlace currentPlace, DevicePlace newPlace);
    public Task<IEnumerable<DevicePlace>> GetAllPlaces();
    public Task<IEnumerable<DevicePlace>> GetFreePlaces();
}