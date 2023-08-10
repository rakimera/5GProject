using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class EnergyFlowService : IEnergyFlowService
{
    public decimal PowerDensitySummation(EnergyResult energyResult)
    {
        throw new NotImplementedException();
    }

    public EnergyResult PowerDensity(decimal powerSignal, decimal gain, decimal transmitLossFactor)
    {
        throw new NotImplementedException();
    }

    public decimal EuclideanDistance()
    {
        throw new NotImplementedException();
    }

    public decimal NormalizedVerticalPower()
    {
        throw new NotImplementedException();
    }
}