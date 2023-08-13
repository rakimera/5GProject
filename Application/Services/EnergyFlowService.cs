using Application.DataObjects;
using Application.Interfaces;
using Application.Models.EnergyResult;
using Domain.Entities;

namespace Application.Services;

public class EnergyFlowService : IEnergyFlowService
{
    private const int GroundInfluenceFactor = 1;
    private const int HumanHeight = 2;
    private readonly int[] _distances;

    public EnergyFlowService()
    {
        _distances = new []{5, 10, 20, 30, 40, 60, 80, 100};
    }

    public List<TotalFluxDensity> PowerDensitySummation(EnergyResult energyResult)
    {
        throw new NotImplementedException();
    }

    public List<EnergyResult> PowerDensity(decimal powerSignal, decimal gain, decimal transmitLossFactor, decimal heightInstall,Guid antennaTranslatorId)
    {
        List<EnergyResult> energyResults = new List<EnergyResult>();
        decimal gainInMultiplier = Multiplier(gain);
        decimal transmitLossFactorInMultiplier = Multiplier(transmitLossFactor);

        foreach (var distance in _distances)
        {
            decimal normalizedVerticalPowerResult = (decimal)Math.Pow((double)NormalizedVerticalPower(), 2);
            decimal euclideanDistanceResult = (decimal)Math.Pow((double)EuclideanDistance(heightInstall, distance), 2);
            
            var result = 8 * powerSignal * gainInMultiplier * GroundInfluenceFactor * transmitLossFactorInMultiplier *
                normalizedVerticalPowerResult / euclideanDistanceResult;
            
            EnergyResult energyResult = new EnergyResult
            {
                Distance = distance,
                Value = result,
                AntennaTranslatorId = antennaTranslatorId
            };
            energyResults.Add(energyResult);
        }

        return energyResults;
    }

    private decimal EuclideanDistance(decimal heightInstall, int distance) //R,m
    {
        var result = (decimal)Math.Sqrt(Math.Pow((double)(heightInstall - HumanHeight), 2) + Math.Pow(distance, 2));
        return result;
    }

    private decimal NormalizedVerticalPower() //F(θ)
    {
        throw new NotImplementedException();
    }
    
    private decimal Multiplier(decimal value) //перевод в разы
    {
        return (decimal)Math.Pow((double)value / 10, 10);
    }

    public BaseResponse<IEnumerable<EnergyResultDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<string>> CreateAsync(EnergyResultDto model, string creator)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<EnergyResultDto>> GetByOid(string oid)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<bool>> Delete(string oid)
    {
        throw new NotImplementedException();
    }
}