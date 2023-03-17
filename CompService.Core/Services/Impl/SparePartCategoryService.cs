using CompService.Core.Messages;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class SparePartCategoryService : IReferenceService<SparePartCategory>
{
    private readonly IReferenceRepository<SparePartCategory> _sourceRepository;

    public SparePartCategoryService(IReferenceRepository<SparePartCategory> sourceRepository)
    {
        _sourceRepository = sourceRepository;
    }
    public async Task<BaseResult> Create(string newRefName)
    {
        var newRef = new SparePartCategory
        {
            Name = newRefName
        };
        await _sourceRepository.Create(newRef);
        return new BaseResult {Success = true};
    }

    public async Task<SparePartCategory?> GetReferenceById(string? id)
    {
        return await _sourceRepository.GetReferenceById(id);
    }

    public async Task<BaseResult> UpdateReference(SparePartCategory? currentRef, string newRefName)
    {
        if (currentRef is null)
        {
            return new BaseResult {Success = false, Message = ReferencesMessages.Error};
        }

        var newRef = new SparePartCategory
        {
            CategoryId = currentRef.CategoryId,
            Name = newRefName
        };
        await _sourceRepository.UpdateReference(currentRef, newRef);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<SparePartCategory>> GetAllValues()
    {
        return await _sourceRepository.GetAllValues();
    }
}