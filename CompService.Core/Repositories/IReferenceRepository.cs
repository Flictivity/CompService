namespace CompService.Core.Repositories;

public interface IReferenceRepository<TReference>
{
    public Task Create(TReference newRef);
    public Task<TReference?> GetReferenceById(string? id);
    public Task UpdateReference(TReference currentRef, TReference newRef);
    
    public Task<IEnumerable<TReference>> GetAllValues();
}