using Domain.Entities;

namespace Application.Interfaces;

public interface IEnergyFlowService
{
    List<TotalFluxDensity> PowerDensitySummation(EnergyResult energyResult);
}