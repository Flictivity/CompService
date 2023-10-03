using CompService.Core.Messages;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class SourceService : IReferenceService<Source>
{
    private readonly IReferenceRepository<Source> _sourceRepository;

    public SourceService(IReferenceRepository<Source> sourceRepository)
    {
        _sourceRepository = sourceRepository;
    }

    public async Task<BaseResult> CreateAsync(string newRefName)
    {
        var newRef = new Source
        {
            Name = newRefName
        };
        await _sourceRepository.Create(newRef);
        return new BaseResult {Success = true};
    }

    public async Task<Source?> GetReferenceByIdAsync(string? id)
    {
        return await _sourceRepository.GetReferenceById(id);
    }

    public async Task<BaseResult> UpdateReferenceAsync(Source? currentRef, string newRefName)
    {
        if (currentRef is null)
        {
            return new BaseResult {Success = false, Message = ReferencesMessages.Error};
        }

        var newRef = new Source
        {
            SourceId = currentRef.SourceId,
            Name = newRefName
        };
        await _sourceRepository.UpdateReference(currentRef, newRef);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<Source>> GetAllValuesAsync()
    {
        return await _sourceRepository.GetAllValues();
    }
}