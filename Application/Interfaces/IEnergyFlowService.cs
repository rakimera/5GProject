using Application.Interfaces.Common;
using Application.Models.EnergyResult;
using Domain.Entities;

namespace Application.Interfaces;

public interface IEnergyFlowService : ICrudService<EnergyResultDto>
{
    List<TotalFluxDensity> PowerDensitySummation(EnergyResult energyResult);
    List<EnergyResult> PowerDensity(decimal powerSignal, decimal gain, decimal transmitLossFactor, decimal heightInstall,Guid antennaTranslatorId);
}