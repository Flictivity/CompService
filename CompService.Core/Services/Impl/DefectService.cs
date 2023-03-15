using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class DefectService : IReferenceService<Defect>
{
    private readonly IReferenceRepository<Defect> _defectRepository;
    public DefectService(IReferenceRepository<Defect> defectRepository)
    {
        _defectRepository = defectRepository;
    }
    public async Task<BaseResult> Create(string newRefName)
    {
        var newRef = new Defect
        {
            Name = newRefName
        };
        await _defectRepository.Create(newRef);
        
        return new BaseResult{Success = true};
    }

    public async Task<Defect?> GetReferenceById(string? id)
    {
        return await _defectRepository.GetReferenceById(id);
    }

    public async Task<BaseResult> UpdateReference(Defect? currentRef, string newRefName)
    {
        if (currentRef is null)
        {
            return new BaseResult {Success = false, Message = "Не удалось изменить данные"};
        }
        var newRef = new Defect()
        {
            DefectId = currentRef.DefectId,
            Name = newRefName
        };
        await _defectRepository.UpdateReference(currentRef, newRef);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<Defect>> GetAllValues()
    {
        return await _defectRepository.GetAllValues();
    }
}