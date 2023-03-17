using CompService.Core.Models;

namespace CompService.Core.Repositories;

public interface IFacilityRepository
{
    public Task CreateFacility(Facility facility);
    public Task<Facility?> GetFacilityById(string? id);
    public Task UpdateFacility(Facility? currentFacility, Facility newFacility);
    public Task<IEnumerable<Facility>> GetAllFacilities();
}