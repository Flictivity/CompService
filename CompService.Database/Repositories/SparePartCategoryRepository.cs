using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Repositories;

public class SparePartCategoryRepository : IReferenceRepository<SparePartCategory>
{
    private readonly IMongoCollection<SparePartCategoryDb> _categories;
    private readonly ILogger<SparePartCategoryRepository> _logger;
    
    public SparePartCategoryRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<SparePartCategoryRepository> logger)
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
            : EntityConverter.ConvertSparePartCategory(res);
    }

    public async Task UpdateReference(SparePartCategory currentRef, SparePartCategory newRef)
    {
        var newDbRef = EntityConverter.ConvertSparePartCategory(newRef);

        await _categories.ReplaceOneAsync(x => x.CategoryId == currentRef.CategoryId, newDbRef);
    }

    public async Task<IEnumerable<SparePartCategory>> GetAllValues()
    {
        var categories = (await _categories.FindAsync(x => true)).ToList();

        return categories.Select(category => 
            new SparePartCategory {CategoryId = category.CategoryId, Name = category.Name,}).ToList();
    }
}