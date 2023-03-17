using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services;

public interface IFacilityService
{
    public Task<BaseResult> Create(Facility newFacility);
    public Task<Facility?> GetFacilityById(string? id);
    public Task<BaseResult> UpdateFacility(Facility? currentFacility, string name, double cost);
    
    public Task<IEnumerable<Facility>> GetAllFacilities();
}