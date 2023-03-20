using CompService.Core.Messages;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class SparePartCategoryService : IReferenceService<SparePartCategory>
{
    private readonly IReferenceRepository<SparePartCategory> _categoriesRepository;

    public SparePartCategoryService(IReferenceRepository<SparePartCategory> categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }
    public async Task<BaseResult> Create(string newRefName)
    {
        var newRef = new SparePartCategory
        {
            Name = newRefName
        };
        await _categoriesRepository.Create(newRef);
        return new BaseResult {Success = true};
    }

    public async Task<SparePartCategory?> GetReferenceById(string? id)
    {
        return await _categoriesRepository.GetReferenceById(id);
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
        await _categoriesRepository.UpdateReference(currentRef, newRef);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<SparePartCategory>> GetAllValues()
    {
        return await _categoriesRepository.GetAllValues();
    }
}