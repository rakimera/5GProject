namespace Application.Interfaces;

public interface IEnergyFlowService
{
    decimal EnergyFlowSummation();
    decimal EnergyFlowOnLevels();
    decimal RMaxBoz();
    decimal EuclideanDistanceCalculation(); //R,m
    decimal AntennaRadiationPattern();
}