using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services;

public interface IFacilityService
{
    public Task<BaseResult> CreateAsync(Facility newFacility);
    public Task<Facility?> GetFacilityByIdAsync(string? id);
    public Task<BaseResult> UpdateFacilityAsync(Facility currentFacility, string name, double cost);
    
    public Task<IEnumerable<Facility>> GetAllFacilitiesAsync();
}