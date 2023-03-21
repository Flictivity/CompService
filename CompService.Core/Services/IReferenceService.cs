using CompService.Core.Results;

namespace CompService.Core.Services;

public interface IReferenceService<TReference>
{
    public Task<BaseResult> CreateAsync(string newRefName);
    public Task<TReference?> GetReferenceByIdAsync(string? id);
    public Task<BaseResult> UpdateReferenceAsync(TReference currentRef, string newRefName);
    
    public Task<IEnumerable<TReference>> GetAllValuesAsync();
}