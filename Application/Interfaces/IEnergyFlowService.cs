using Application.DataObjects;
using Application.Models.EnergyResult;
using Domain.Entities;

namespace Application.Interfaces;

public interface IEnergyFlowService
{
    Task<BaseResponse<string>> CreateAsync(string projectId, string creator);
    BaseResponse<List<EnergyResultDto>> GetAllByOid(string id);
    Task<BaseResponse<bool>> Delete(string id);
    List<CreateTotalFluxDensityDto> PowerDensitySummation(List<EnergyResult> energyResult);
    decimal Multiplier(decimal value);

    decimal GetRB(decimal power, decimal height, decimal lost, decimal multiplier);
    decimal GetRZ(decimal degree, decimal rB);
    decimal GetRX(decimal degree, decimal rB);
}