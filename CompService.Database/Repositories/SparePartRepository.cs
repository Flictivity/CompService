using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Repositories;

public class SparePartRepository : ISparePartRepository
{
    private readonly IMongoCollection<SparePartDb> _spareParts;
    private readonly ILogger<SparePartRepository> _logger;
    private readonly IReferenceRepository<SparePartCategory> _sparePartCategoryRepository;

    public SparePartRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<SparePartRepository> logger, IReferenceRepository<SparePartCategory> sparePartCategoryRepository)
    {
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _spareParts = mongoDatabase.GetCollection<SparePartDb>(
            databaseConnectionSettings.Value.SparePartsCollectionName);
        _logger = logger;
        _sparePartCategoryRepository = sparePartCategoryRepository;
    }

    public async Task Create(SparePart newSparePart)
    {
        var sparePartDb = EntityConverter.ConvertSparePart(newSparePart);
        await _spareParts.InsertOneAsync(sparePartDb);
    }

    public async Task<SparePart?> GetSparePartByName(string? name)
    {
        var res = (await _spareParts.FindAsync(x => x.Name == name)).FirstOrDefault();
        if (res is null)
        {
            return null;
        }

        var category = await _sparePartCategoryRepository.GetReferenceById(res.CategoryId);
        var sparePart = EntityConverter.ConvertSparePart(res);
        if (category is null)
        {
            return sparePart;
        }

        sparePart.Category = category;

        return sparePart;
    }

    public async Task<SparePart?> GetSparePartById(string? id)
    {
        var res = (await _spareParts.FindAsync(x => x.SparePartId == id)).FirstOrDefault();
        if (res is null)
        {
            return null;
        }

        var category = await _sparePartCategoryRepository.GetReferenceById(res.CategoryId);
        var sparePart = EntityConverter.ConvertSparePart(res);
        if (category is null)
        {
            return sparePart;
        }

        sparePart.Category = category;

        return sparePart;
    }

    public async Task<SparePart?> GetSparePartByArticle(string? article)
    {
        var res = (await _spareParts.FindAsync(x => x.Article == article)).FirstOrDefault();
        if (res is null)
        {
            return null;
        }

        var category = await _sparePartCategoryRepository.GetReferenceById(res.CategoryId);
        var sparePart = EntityConverter.ConvertSparePart(res);
        if (category is null)
        {
            return sparePart;
        }

        sparePart.Category = category;

        return sparePart;
    }

    public async Task UpdateSparePart(SparePart currentSparePart, SparePart newSparePart)
    {
        var newSparePartDb = new SparePartDb
        {
            SparePartId = currentSparePart.SparePartId,
            Name = newSparePart.Name,
            Article = newSparePart.Article,
            CategoryId = newSparePart.Category.CategoryId,
            Count = newSparePart.Count,
            PurchasePrice = newSparePart.PurchasePrice,
            RetailPrice = newSparePart.RetailPrice
        };

        await _spareParts
            .ReplaceOneAsync(x => x.SparePartId == currentSparePart.SparePartId, newSparePartDb);
    }

    public async Task<IEnumerable<SparePart>> GetAllSpareParts()
    {
        var res = new List<SparePart>();
        var spareParts = (await _spareParts.FindAsync(x => true)).ToList();

        foreach (var sparePart in spareParts)
        {
            var category = await _sparePartCategoryRepository.GetReferenceById(sparePart.CategoryId);
            var newSparePart = EntityConverter.ConvertSparePart(sparePart);
            if (category is not null)
            {
                newSparePart.Category = category;
            }

            res.Add(newSparePart);
        }

        return res;
    }
}