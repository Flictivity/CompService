using CompService.Core.Messages;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class AppearanceService : IReferenceService<Appearance>
{
    private readonly IReferenceRepository<Appearance> _defectRepository;
    public AppearanceService(IReferenceRepository<Appearance> defectRepository)
    {
        _defectRepository = defectRepository;
    }
    public async Task<BaseResult> CreateAsync(string newRefName)
    {
        var newRef = new Appearance
        {
            Name = newRefName
        };
        await _defectRepository.Create(newRef);
        
        return new BaseResult{Success = true};
    }

    public async Task<Appearance?> GetReferenceByIdAsync(string? id)
    {
        return await _defectRepository.GetReferenceById(id);
    }

    public async Task<BaseResult> UpdateReferenceAsync(Appearance? currentRef, string newRefName)
    {
        if (currentRef is null)
        {
            return new BaseResult {Success = false, Message = ReferencesMessages.Error};
        }
        var newRef = new Appearance
        {
            AppearanceId = currentRef.AppearanceId,
            Name = newRefName
        };
        await _defectRepository.UpdateReference(currentRef, newRef);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<Appearance>> GetAllValuesAsync()
    {
        return await _defectRepository.GetAllValues();
    }
}