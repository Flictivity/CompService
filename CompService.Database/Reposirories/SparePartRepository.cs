using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using CompService.Database.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Reposirories;

public class SparePartRepository : ISparePartRepository
{
    private readonly IMongoCollection<SparePartDb> _spareParts;
    private readonly ILogger<ISparePartRepository> _logger;

    public SparePartRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<ISparePartRepository> logger)
    {
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _spareParts = mongoDatabase.GetCollection<SparePartDb>(
            databaseConnectionSettings.Value.SparePartsCollectionName);
        _logger = logger;
    }
    public async Task Create(SparePart newSparePart)
    {
        var sparePartDb = EntityConverter.ConvertSparePart(newSparePart);
        await _spareParts.InsertOneAsync(sparePartDb);
    }

    public async Task<SparePart?> GetSparePartByName(string? name)
    {
        var res = (await _spareParts.FindAsync(x => x.Name == name)).FirstOrDefault();
        return res is null ? null : EntityConverter.ConvertSparePart(res);
    }

    public async Task<SparePart?> GetSparePartById(string? id)
    {
        var res = (await _spareParts.FindAsync(x => x.SparePartId == id)).FirstOrDefault();
        return res is null ? null : EntityConverter.ConvertSparePart(res);
    }

    public async Task<SparePart?> GetSparePartByArticle(string? article)
    {
        var res = (await _spareParts.FindAsync(x => x.Article == article)).FirstOrDefault();
        return res is null ? null : EntityConverter.ConvertSparePart(res);
    }

    public async Task UpdateSparePart(SparePart currentSparePart, SparePart newSparePart)
    {
        var newSparePartDb = new SparePartDb
        {
            SparePartId = currentSparePart.SparePartId,
            Name = newSparePart.Name,
            Article = newSparePart.Article,
            Category = new SparePartCategoryDb
            {
                CategoryId = newSparePart.Category.CategoryId,
                Name = newSparePart.Category.Name
            },
            Count = newSparePart.Count,
            PurchasePrice = newSparePart.PurchasePrice,
            RetailPrice = newSparePart.RetailPrice
        };
            
        await _spareParts.ReplaceOneAsync(x => x.SparePartId == currentSparePart.SparePartId, newSparePartDb);
    }

    public async Task<IEnumerable<SparePart>> GetAllSpareParts()
    {
        var spareParts = (await _spareParts.FindAsync(x => true)).ToList();
        var res = new List<SparePart>();
        foreach (var sparePart in spareParts)
        {
            res.Add(EntityConverter.ConvertSparePart(sparePart));
        }

        return res;
    }
}