using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.EnergyResult;
using Application.Validation;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

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

    public List<CreateTotalFluxDensityDto> PowerDensitySummation(List<EnergyResult> energyResults)
    {
        List<CreateTotalFluxDensityDto> totalFluxDensities = new List<CreateTotalFluxDensityDto>();

        foreach (var distance in _distances)
        {
            var totalFluxDensity = new CreateTotalFluxDensityDto { Distance = distance };
            foreach (var energyResult in energyResults)
            {
                if (energyResult.Distance == distance)
                {
                    totalFluxDensity.Value += energyResult.Value;
                }
            }
            totalFluxDensities.Add(totalFluxDensity);
        }

        return totalFluxDensities;
    }

    private async Task<List<EnergyResult>> PowerDensity(CreateEnergyResultDto inputData)
    {
        List<EnergyResult> energyResults = new List<EnergyResult>();
        decimal gainInMultiplier = Multiplier(inputData.Gain);
        decimal transmitLossFactorInMultiplier = Multiplier(inputData.TransmitLossFactor);
        decimal heightInstall = inputData.HeightInstall;
        decimal powerSignal = inputData.PowerSignal;
        AntennaTranslator? antennaTranslator =
           await _repositoryWrapper.AntennaTranslatorRepository.GetByCondition(x => x.Id == inputData.AntennaTranslatorId);
        var translatorId = antennaTranslator.TranslatorSpecsId;
        
        foreach (var distance in _distances)
        {
            decimal normalizedVerticalPowerResult = (decimal)Math.Pow((double)NormalizedVerticalPower(distance, heightInstall, translatorId), 2);
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

    public decimal EuclideanDistance(decimal heightInstall, int distance) //R,m
    {
        var result = (decimal)Math.Sqrt(Math.Pow((double)(heightInstall - HumanHeight), 2) + Math.Pow(distance, 2));
        return result;
    }
    public decimal EuclideanDistanceDecimal(decimal power,decimal height,decimal lost,decimal multiplier) //R,m
    {
        var info = Math.Sqrt(8 * 25 * 44.668 * 0.849 / 10) * 1 * 0.000;
        Console.WriteLine(info);
        double rSqrt = Math.Sqrt(8 * (double)power * (double)Multiplier(height) * (double)Multiplier(-lost) / 10) * 1 * (double)Multiplier(multiplier);
        double result = Math.Round(rSqrt, 3);
        return (decimal)result;
    }

    private decimal NormalizedVerticalPower(decimal distance, decimal heightInstall, Guid translatorId) //F(θ)
    {
        int degree = (int)Math.Ceiling(Math.Atan((double)((heightInstall - HumanHeight) / distance)) * 180 / Math.PI);
        var radiationZones = _repositoryWrapper.RadiationZoneRepository.GetAllByCondition(x => x.Id == translatorId).Where(x=> x.DirectionType == DirectionType.Vertical);
        RadiationZone? verticalRadiation = radiationZones.FirstOrDefault(x => x.Degree == degree);
        var verticalRadiationInMultiplier = Multiplier(verticalRadiation.Value);
        
        return verticalRadiationInMultiplier;
    }

    public async Task<BaseResponse<string>> CreateAsync(CreateEnergyResultDto createEnergyResultDto, string creator)
    {
        var validationResult = await _energyResultValidator.ValidateAsync(createEnergyResultDto);
        if (validationResult.IsValid)
        {
            List<EnergyResult> calculationResults = await PowerDensity(createEnergyResultDto);
            
            foreach (var calculationResult in calculationResults)
            {
                calculationResult.CreatedBy = creator;
                await _repositoryWrapper.EnergyFlowRepository.CreateAsync(calculationResult);
            }

            await _repositoryWrapper.Save();
            return new BaseResponse<string>(
                Result: "",
                Success: true,
                Messages: new List<string>{"Просчеты плотности потока энергии успешно созданы"});
        }
        
        List<string> messages = _mapper.Map<List<string>>(validationResult.Errors);
        return new BaseResponse<string>(
            Result: "", 
            Messages: messages,
            Success: false);
    }

    public BaseResponse<List<EnergyResultDto>> GetAllByOid(string oid)
    {
        var energyResults =  _repositoryWrapper.EnergyFlowRepository.GetAllByCondition(x => x.AntennaTranslatorId.ToString() == oid);
        List<EnergyResultDto> model = _mapper.Map<List<EnergyResultDto>>(energyResults);
        if (energyResults is null)
            return new BaseResponse<List<EnergyResultDto>>(
                Result: null,
                Messages: new List<string>{"Просчеты плотности потока энергии не найдены"},
                Success: true);
        return new BaseResponse<List<EnergyResultDto>>(
            Result: model,
            Success: true,
            Messages: new List<string>{"Просчеты плотности потока энергии успешно найдены"});
    }

    public async Task<BaseResponse<bool>> Delete(List<EnergyResult> energyResults)
    {
        if (energyResults.Count > 0)
        {
            foreach (var energyResult in energyResults)
            {
                energyResult.IsDelete = true;
                _repositoryWrapper.EnergyFlowRepository.Update(energyResult);
            }
            
            await _repositoryWrapper.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string>{"Просчеты плотности потока энергии успешно удалены"});
        }
        return new BaseResponse<bool>(
            Result: false, 
            Messages: new List<string>{"Просчеты плотности потока энергии не существуют"},
            Success: false);
    }
    
    public decimal Multiplier(decimal value) //перевод в разы
    {
        double baseNumber = 10;
        double exponent = (double)value / baseNumber;

        double result = Math.Pow(baseNumber, exponent);
        return (decimal)result;
    }
}