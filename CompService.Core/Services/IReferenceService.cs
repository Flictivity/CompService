using CompService.Core.Results;

namespace CompService.Core.Services;

public interface IReferenceService<TReference>
{
    public Task<BaseResult> Create(string newRefName);
    public Task<TReference?> GetReferenceById(string? id);
    public Task<BaseResult> UpdateReference(TReference? currentRef, string newRefName);
    
    public Task<IEnumerable<TReference>> GetAllValues();
}