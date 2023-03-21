using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Repositories;

public class BrandRepository : IReferenceRepository<Brand>
{
    private readonly IMongoCollection<BrandDb> _brands;
    private readonly ILogger<BrandRepository> _logger;
    public BrandRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<BrandRepository> logger)
    {
        _logger = logger;
        
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _brands = mongoDatabase.GetCollection<BrandDb>(
            databaseConnectionSettings.Value.BrandsCollectionName);
    }
    
    public async Task Create(Brand newRef)
    {
        var dbRef = new BrandDb
        {
            Name = newRef.Name
        };
        await _brands.InsertOneAsync(dbRef);
    }

    public async Task<Brand?> GetReferenceById(string? id)
    {
        var res = (await _brands.FindAsync(x => x.BrandId == id)).FirstOrDefault();

        return res is null
            ? null
            : new Brand
            {
                BrandId = res.BrandId,
                Name = res.Name
            };
    }

    public async Task UpdateReference(Brand currentRef, Brand newRef)
    {
        var newDbRef = new BrandDb
        {
            BrandId = newRef.BrandId,
            Name = newRef.Name
        };

        await _brands.ReplaceOneAsync(x => x.BrandId == currentRef.BrandId, newDbRef);
    }

    public async Task<IEnumerable<Brand>> GetAllValues()
    {
        var defects = (await _brands.FindAsync(x => true)).ToList();

        return defects.Select(reference => 
            new Brand {BrandId = reference.BrandId, Name = reference.Name,}).ToList();
    }
}