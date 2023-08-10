using Domain.Entities;

namespace Application.Interfaces;

public interface   IEnergyFlowService
{
    decimal PowerDensitySummation(EnergyResult energyResult);
    EnergyResult PowerDensity(decimal powerSignal, decimal gain, decimal transmitLossFactor);
    decimal EuclideanDistance(); //R,m
    decimal NormalizedVerticalPower(); //F(θ)
}