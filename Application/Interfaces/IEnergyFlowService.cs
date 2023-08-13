using Application.DataObjects;
using Application.Models.EnergyResult;
using Domain.Entities;

namespace Application.Interfaces;

public interface IEnergyFlowService
{
    Task<BaseResponse<string>> CreateAsync(CreateEnergyResultDto createEnergyResultDto, string creator);
    BaseResponse<List<EnergyResultDto>> GetAllByOid(string oid);
    Task<BaseResponse<bool>> Delete(List<EnergyResult> energyResults);
    List<TotalFluxDensity> PowerDensitySummation(EnergyResult energyResult);
}