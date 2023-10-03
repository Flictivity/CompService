using CompService.Core.Messages;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class DeviceTypeService : IReferenceService<DeviceType>
{
    private readonly IReferenceRepository<DeviceType> _defectRepository;
    public DeviceTypeService(IReferenceRepository<DeviceType> defectRepository)
    {
        _defectRepository = defectRepository;
    }
    public async Task<BaseResult> CreateAsync(string newRefName)
    {
        var newRef = new DeviceType
        {
            Name = newRefName
        };
        await _defectRepository.Create(newRef);
        
        return new BaseResult{Success = true};
    }

    public async Task<DeviceType?> GetReferenceByIdAsync(string? id)
    {
        return await _defectRepository.GetReferenceById(id);
    }

    public async Task<BaseResult> UpdateReferenceAsync(DeviceType? currentRef, string newRefName)
    {
        if (currentRef is null)
        {
            return new BaseResult {Success = false, Message = ReferencesMessages.Error};
        }
        var newRef = new DeviceType
        {
            DeviceTypeId = currentRef.DeviceTypeId,
            Name = newRefName
        };
        await _defectRepository.UpdateReference(currentRef, newRef);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<DeviceType>> GetAllValuesAsync()
    {
        return await _defectRepository.GetAllValues();
    }
}