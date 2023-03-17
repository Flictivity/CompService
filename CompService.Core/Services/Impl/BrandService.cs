using CompService.Core.Messages;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class BrandService : IReferenceService<Brand>
{
    private readonly IReferenceRepository<Brand> _brandRepository;
    public BrandService(IReferenceRepository<Brand> brandRepository)
    {
        _brandRepository = brandRepository;
    }
    public async Task<BaseResult> Create(string newRefName)
    {
        var newRef = new Brand
        {
            Name = newRefName
        };
        await _brandRepository.Create(newRef);
        
        return new BaseResult{Success = true};
    }

    public async Task<Brand?> GetReferenceById(string? id)
    {
        return await _brandRepository.GetReferenceById(id);
    }

    public async Task<BaseResult> UpdateReference(Brand? currentRef, string newRefName)
    {
        if (currentRef is null)
        {
            return new BaseResult {Success = false, Message = ReferencesMessages.Error};
        }
        var newRef = new Brand
        {
            BrandId = currentRef.BrandId,
            Name = newRefName
        };
        await _brandRepository.UpdateReference(currentRef, newRef);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<Brand>> GetAllValues()
    {
        return await _brandRepository.GetAllValues();
    }
}