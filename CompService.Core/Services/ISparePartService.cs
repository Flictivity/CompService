using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services;

public interface ISparePartService
{
    public Task<BaseResult> CreateAsync(SparePart newSparePart);
    public Task<SparePart?> GetSparePartByIdAsync(string? id);
    public Task<SparePart?> GetSparePartByNameAsync(string? name);
    public Task<SparePart?> GetSparePartByArticleAsync(string? article);
    public Task<BaseResult> UpdateSparePartAsync(SparePart currentSparePart, SparePart newSparePart);
    public Task<BaseResult> UpdateSparePartCountAsync(string id, int newCount);
    public Task<IEnumerable<SparePart>> GetAllSparePartsAsync();
}