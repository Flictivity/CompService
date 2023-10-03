using CompService.Core.Models;

namespace CompService.Core.Repositories;

public interface ISparePartRepository
{
    public Task Create(SparePart newSparePart);
    public Task<SparePart?> GetSparePartByName(string? name);
    public Task<SparePart?> GetSparePartById(string? id);
    public Task<SparePart?> GetSparePartByArticle(string? article);
    public Task UpdateSparePart(SparePart currentSparePart, SparePart newSparePart);
    public Task<IEnumerable<SparePart>> GetAllSpareParts();
    public Task UpdateCount(string sparePartId, int count);
}