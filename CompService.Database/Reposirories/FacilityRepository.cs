﻿using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Database.Models;
using CompService.Database.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Reposirories;

public class FacilityRepository : IFacilityRepository
{
    private readonly IMongoCollection<FacilityDb> _facilities;
    private readonly ILogger<FacilityRepository> _logger;

    public FacilityRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<FacilityRepository> logger)
    {
        _logger = logger;

        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _facilities = mongoDatabase.GetCollection<FacilityDb>(
            databaseConnectionSettings.Value.FacilitiesCollectionName);
    }

    public async Task CreateFacility(Facility facility)
    {
        var dbRef = new FacilityDb
        {
            Name = facility.Name,
            Cost = facility.Cost
        };
        await _facilities.InsertOneAsync(dbRef);
    }

    public async Task<Facility?> GetFacilityById(string? id)
    {
        var res = (await _facilities.FindAsync(x => x.FacilityId == id)).FirstOrDefault();

        return res is null
            ? null
            : new Facility
            {
                FacilityId = res.FacilityId,
                Name = res.Name,
                Cost = res.Cost
            };
    }

    public async Task UpdateFacility(Facility? currentFacility, Facility newFacility)
    {
        if (currentFacility is null)
        {
            _logger.LogError("Null reference");
            return;
        }

        var newDbFacility = new FacilityDb
        {
            FacilityId = newFacility.FacilityId,
            Name = newFacility.Name,
            Cost = newFacility.Cost,
        };

        await _facilities.ReplaceOneAsync(x => x.FacilityId == currentFacility.FacilityId, newDbFacility);
    }

    public async Task<IEnumerable<Facility>> GetAllFacilities()
    {
        var facilities = (await _facilities.FindAsync(x => true)).ToList();
        var res = new List<Facility>();
        foreach (var facility in facilities)
        {
            res.Add(new Facility
            {
                FacilityId = facility.FacilityId,
                Name = facility.Name,
                Cost = facility.Cost
            });
        }

        return res;
    }
}