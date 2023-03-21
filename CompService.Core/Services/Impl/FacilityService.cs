using CompService.Core.Messages;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class FacilityService : IFacilityService
{
    private readonly IFacilityRepository _facilityRepository;

    public FacilityService(IFacilityRepository facilityRepository)
    {
        _facilityRepository = facilityRepository;
    }

    public async Task<BaseResult> Create(Facility newFacility)
    {
        await _facilityRepository.CreateFacility(newFacility);

        return new BaseResult {Success = true};
    }

    public async Task<Facility?> GetFacilityById(string? id)
    {
        return await _facilityRepository.GetFacilityById(id);
    }

    public async Task<BaseResult> UpdateFacility(Facility currentFacility, string name, double cost)
    {
        var newFacility = new Facility
        {
            Name = name,
            Cost = cost,
            FacilityId = currentFacility.FacilityId
        };

        await _facilityRepository.UpdateFacility(currentFacility, newFacility);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<Facility>> GetAllFacilities()
    {
        return await _facilityRepository.GetAllFacilities();
    }
}