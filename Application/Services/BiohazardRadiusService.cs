using Application.DataObjects;
using Application.Extensions;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class BiohazardRadiusService : IBiohazardRadiusService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public BiohazardRadiusService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<BaseResponse<bool>> Create(string id)
    {
        var projectAntennae = _repositoryWrapper.ProjectAntennaRepository.GetAllByCondition(x => x.Id.ToString() == id).ToList();
        foreach (var projectAntenna in projectAntennae)
        {
            var antennaTranslators = _repositoryWrapper
                .AntennaTranslatorRepository.GetAllByCondition(x=>x.ProjectAntenna == projectAntenna).ToList();
            foreach (var value in antennaTranslators)
            {
                var translator = value.TranslatorSpecs;
                var radiationZonesHorizontal = _repositoryWrapper.RadiationZoneRepository
                    .GetAllByCondition(x=> x.TranslatorSpecsId == translator.Id 
                                           && x.DirectionType == DirectionType.Horizontal.GetDescription() && x.Degree != 360)
                    .OrderBy(x=>x.Degree).ToList();
                var radiationZonesVertical = _repositoryWrapper.RadiationZoneRepository
                    .GetAllByCondition(x=> x.TranslatorSpecsId == translator.Id 
                                           && x.DirectionType == DirectionType.Vertical.GetDescription() && x.Degree != 360)
                    .OrderBy(x=>x.Degree).ToList();
                await BioHazardCreate(radiationZonesHorizontal, projectAntenna, value);
                await BioHazardCreate(radiationZonesVertical, projectAntenna, value);
            }
            
        }
        
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Рассчеты произведены успешно успешно считан" },
            Success: true);
    }
    public decimal GetRB(decimal power,decimal height,decimal lost,decimal multiplier) //Rb,m
    {
        var h = Multiplier(height);
        var l = Multiplier(-lost);
        var m = Multiplier(multiplier);
        double rB = Math.Sqrt(8 * (double)power * (double)h * (double)l / 10) * 1 * (double)multiplier;
        double result = Math.Round(rB, 3);
        return (decimal)result;
    }
    
    
    public decimal GetRZ(decimal degree,decimal rB) //Rz,m
    {
        double rZ = (double)rB * Math.Sin((double)-degree * Math.PI / 180);
        double result = Math.Round(rZ, 3);
        return (decimal)result;
    }
    
    public decimal GetRX(decimal degree,decimal rB) //Rx,m
    {
        double rZ = (double)rB * Math.Cos((double)-degree * Math.PI / 180);
        double result = Math.Round(rZ, 3);
        return (decimal)result;
    }
    public decimal Multiplier(decimal value) //перевод в разы
    {
        double baseNumber = 10;
        double exponent = (double)value / baseNumber;

        double result = Math.Pow(baseNumber, exponent);
        return (decimal)result;
    }

    private async Task<bool> BioHazardCreate(List<RadiationZone> radiationZones,ProjectAntenna projectAntenna, AntennaTranslator antennaTranslator )
    {
        var tilt = (int)projectAntenna.Tilt;
        for (int i = 0; i < radiationZones.Count; i++)
        {
            int newIndex = (i + tilt) % (radiationZones.Count);
            var dbRaz = Multiplier(radiationZones[newIndex].Value);
            var maxRadius = GetRB(antennaTranslator.Power, antennaTranslator.Gain, antennaTranslator.TransmitLossFactor, dbRaz);
            BiohazardRadius biohazardRadius = new BiohazardRadius()
            {
                Degree = i,
                Db = radiationZones[newIndex].Value,
                DbRaz = dbRaz,
                MaximumBiohazardRadius = maxRadius,
                BiohazardRadiusX = GetRX(radiationZones[newIndex].Degree, maxRadius),
                BiohazardRadiusZ = GetRZ(radiationZones[newIndex].Degree, maxRadius),
                DirectionType = radiationZones[newIndex].DirectionType,
                AntennaTranslatorId = antennaTranslator.Id
            };
            if (i == 0)
            {
                BiohazardRadius biohazardRadiusMax = new BiohazardRadius()
                {
                    Degree = 360,
                    Db = radiationZones[newIndex].Value,
                    DbRaz = dbRaz,
                    MaximumBiohazardRadius = maxRadius,
                    BiohazardRadiusX = GetRX(radiationZones[newIndex].Degree, maxRadius),
                    BiohazardRadiusZ = GetRZ(radiationZones[newIndex].Degree, maxRadius),
                    DirectionType = radiationZones[newIndex].DirectionType,
                    AntennaTranslatorId = antennaTranslator.Id
                };
                await _repositoryWrapper.BiohazardRadiusRepository.CreateAsync(biohazardRadiusMax);
            }
            await _repositoryWrapper.BiohazardRadiusRepository.CreateAsync(biohazardRadius);
        }
        await _repositoryWrapper.Save();
        return true;
    }
}