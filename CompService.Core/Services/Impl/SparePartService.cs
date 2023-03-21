using CompService.Core.Messages;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class SparePartService : ISparePartService
{
    private readonly ISparePartRepository _sparePartRepository;

    public SparePartService(ISparePartRepository sparePartRepository)
    {
        _sparePartRepository = sparePartRepository;
    }
    public async Task<BaseResult> CreateAsync(SparePart newSparePart)
    {
        var sparePartInDb = await GetSparePartByNameAsync(newSparePart.Name);

        if (sparePartInDb is not null)
        {
            await UpdateSparePartCountAsync(sparePartInDb.SparePartId, sparePartInDb.Count + newSparePart.Count);
            return new BaseResult {Success = true};
        }

        await _sparePartRepository.Create(newSparePart);
        return new BaseResult {Success = true};
    }

    public async Task<SparePart?> GetSparePartByIdAsync(string? id)
    {
        return await _sparePartRepository.GetSparePartById(id);
    }

    public async Task<SparePart?> GetSparePartByNameAsync(string? name)
    {
        return await _sparePartRepository.GetSparePartByName(name);
    }

    public async Task<SparePart?> GetSparePartByArticleAsync(string? article)
    {
        return await _sparePartRepository.GetSparePartById(article);
    }

    public async Task<BaseResult> UpdateSparePartAsync(SparePart currentSparePart, SparePart newSparePart)
    {
        await _sparePartRepository.UpdateSparePart(currentSparePart, newSparePart);
        return new BaseResult {Success = true};
    }

    public async Task<BaseResult> UpdateSparePartCountAsync(string id, int newCount)
    {
        var sparePart = await GetSparePartByIdAsync(id);
        if (sparePart is null)
        {
            return new BaseResult {Success = false, Message = SparePartMessages.NotFound};
        }

        var newSparePart = new SparePart
        {
            SparePartId = sparePart.SparePartId,
            Name = sparePart.Name,
            Article = sparePart.Article,
            Category = sparePart.Category,
            Count = sparePart.Count,
            PurchasePrice = sparePart.PurchasePrice,
            RetailPrice = sparePart.RetailPrice
        };
        await _sparePartRepository.UpdateSparePart(sparePart, newSparePart);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<SparePart>> GetAllSparePartsAsync()
    {
        return await _sparePartRepository.GetAllSpareParts();
    }
}