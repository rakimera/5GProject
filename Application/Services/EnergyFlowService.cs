using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.EnergyResult;
using Application.Validation;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class EnergyFlowService : IEnergyFlowService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly EnergyResultValidator _energyResultValidator;
    private const int GroundInfluenceFactor = 1;
    private const int HumanHeight = 2;
    private readonly int[] _distances;

    public EnergyFlowService(EnergyResultValidator energyResultValidator, IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _energyResultValidator = energyResultValidator;
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _distances = new []{5, 10, 20, 30, 40, 60, 80, 100};
    }

    public List<TotalFluxDensity> PowerDensitySummation(EnergyResult energyResult)
    {
        throw new NotImplementedException();
    }

    private List<EnergyResult> PowerDensity(CreateEnergyResultDto inputData)
    {
        List<EnergyResult> energyResults = new List<EnergyResult>();
        decimal gainInMultiplier = Multiplier(inputData.Gain);
        decimal transmitLossFactorInMultiplier = Multiplier(inputData.TransmitLossFactor);
        decimal heightInstall = inputData.HeightInstall;
        decimal powerSignal = inputData.PowerSignal;
        
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
                AntennaTranslatorId = inputData.AntennaTranslatorId
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

    public BaseResponse<IEnumerable<EnergyResultDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<string>> CreateAsync(CreateEnergyResultDto createEnergyResultDto, string creator)
    {
        
        List<EnergyResult> calculationResult = PowerDensity(createEnergyResultDto);
        
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
    
    private decimal Multiplier(decimal value) //перевод в разы
    {
        return (decimal)Math.Pow((double)value / 10, 10);
    }
}