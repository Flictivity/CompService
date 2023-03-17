using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services;

public interface ISparePartService
{
    public Task<BaseResult> Create(SparePart newSparePart);
    public Task<SparePart?> GetSparePartById(string? id);
    public Task<SparePart?> GetSparePartByName(string? name);
    public Task<SparePart?> GetSparePartByArticle(string? article);
    public Task<BaseResult> UpdateSparePart(SparePart currentSparePart, SparePart newSparePart);
    public Task<BaseResult> UpdateSparePartCount(string id, int newCount);
    public Task<IEnumerable<SparePart>> GetAllSpareParts();
}