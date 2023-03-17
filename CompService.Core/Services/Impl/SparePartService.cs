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
    public async Task<BaseResult> Create(SparePart newSparePart)
    {
        var sparePartInDb = await GetSparePartByName(newSparePart.Name);

        if (sparePartInDb is not null)
        {
            await UpdateSparePartCount(sparePartInDb.SparePartId, sparePartInDb.Count + newSparePart.Count);
            return new BaseResult {Success = true};
        }

        await _sparePartRepository.Create(newSparePart);
        return new BaseResult {Success = true};
    }

    public async Task<SparePart?> GetSparePartById(string? id)
    {
        return await _sparePartRepository.GetSparePartById(id);
    }

    public async Task<SparePart?> GetSparePartByName(string? name)
    {
        return await _sparePartRepository.GetSparePartByName(name);
    }

    public async Task<SparePart?> GetSparePartByArticle(string? article)
    {
        return await _sparePartRepository.GetSparePartById(article);
    }

    public async Task<BaseResult> UpdateSparePart(SparePart currentSparePart, SparePart newSparePart)
    {
        await _sparePartRepository.UpdateSparePart(currentSparePart, newSparePart);
        return new BaseResult {Success = true};
    }

    public async Task<BaseResult> UpdateSparePartCount(string id, int newCount)
    {
        var sparePart = await GetSparePartById(id);
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

    public async Task<IEnumerable<SparePart>> GetAllSpareParts()
    {
        return await _sparePartRepository.GetAllSpareParts();
    }
}