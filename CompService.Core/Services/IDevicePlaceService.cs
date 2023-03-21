﻿using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services;

public interface IDevicePlaceService
{
    public Task<BaseResult> CreateAsync(DevicePlace place);
    public Task<DevicePlace?> GetClientByIdAsync(string id);
    public Task<DevicePlace?> GetClientByOrderAsync(string orderId);

    public Task<BaseResult> UpdateDevicePlaceAsync(DevicePlace currentPlace, DevicePlace newPlace);

    public Task<IEnumerable<DevicePlace>> GetAllPlacesAsync();
}