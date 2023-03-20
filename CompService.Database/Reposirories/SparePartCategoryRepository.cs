using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using CompService.Database.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Reposirories;

public class SparePartCategoryRepository : IReferenceRepository<SparePartCategory>
{
    private readonly IMongoCollection<SparePartCategoryDb> _categories;
    private readonly ILogger<IReferenceRepository<SparePartCategory>> _logger;
    
    public SparePartCategoryRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<IReferenceRepository<SparePartCategory>> logger)
    {
        _logger = logger;
        
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _categories = mongoDatabase.GetCollection<SparePartCategoryDb>(
            databaseConnectionSettings.Value.SparePartCategoriesCollectionName);
    }
    public async Task Create(SparePartCategory newRef)
    {
        var dbRef = new SparePartCategoryDb
        {
            Name = newRef.Name
        };
        await _categories.InsertOneAsync(dbRef);
    }

    public async Task<SparePartCategory?> GetReferenceById(string? id)
    {
        var res = (await _categories.FindAsync(x => x.CategoryId == id)).FirstOrDefault();

        return res is null
            ? null
            : new SparePartCategory
            {
                CategoryId = res.CategoryId,
                Name = res.Name
            };
    }

    public async Task UpdateReference(SparePartCategory? currentRef, SparePartCategory newRef)
    {
        if (currentRef is null)
        {
            _logger.LogError("Null reference");
            return;
        }

        var newDbRef = new SparePartCategoryDb
        {
            CategoryId = newRef.CategoryId,
            Name = newRef.Name
        };

        await _categories.ReplaceOneAsync(x => x.CategoryId == currentRef.CategoryId, newDbRef);
    }

    public async Task<IEnumerable<SparePartCategory>> GetAllValues()
    {
        var categories = (await _categories.FindAsync(x => true)).ToList();
        var res = new List<SparePartCategory>();
        foreach (var category in categories)
        {
            res.Add(new SparePartCategory
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
            });
        }

        return res;
    }
}