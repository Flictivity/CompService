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
    public async Task<BaseResult> Create(string newRefName)
    {
        var newRef = new Appearance
        {
            Name = newRefName
        };
        await _defectRepository.Create(newRef);
        
        return new BaseResult{Success = true};
    }

    public async Task<Appearance?> GetReferenceById(string? id)
    {
        return await _defectRepository.GetReferenceById(id);
    }

    public async Task<BaseResult> UpdateReference(Appearance? currentRef, string newRefName)
    {
        if (currentRef is null)
        {
            return new BaseResult {Success = false, Message = "Не удалось изменить данные"};
        }
        var newRef = new Appearance
        {
            AppearanceId = currentRef.AppearanceId,
            Name = newRefName
        };
        await _defectRepository.UpdateReference(currentRef, newRef);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<Appearance>> GetAllValues()
    {
        return await _defectRepository.GetAllValues();
    }
}