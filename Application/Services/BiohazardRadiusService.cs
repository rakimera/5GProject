using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Domain.Entities;

namespace Application.Services;

public class BiohazardRadiusService : IBiohazardRadiusService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public BiohazardRadiusService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<BaseResponse<bool>> Create(Project project)
    {
        var projectAntennae = project.ProjectAntennae;
        foreach (var projectAntenna in projectAntennae)
        {
            var antennaTranslators = _repositoryWrapper
                .AntennaTranslatorRepository.GetAllByCondition(x=>x.ProjectAntenna == projectAntenna).ToList();
            foreach (var value in antennaTranslators)
            {
                var antenna = value.ProjectAntenna;
                var translator = value.TranslatorSpecs;
                // var lossFactor = value.TransmitLossFactor;
                // var gain = value.Gain;
                // var power = value.Power;
                var radiationZones = translator.RadiationZones;
                foreach (var radiationZone in radiationZones)
                {
                    var dbRaz = Multiplier(radiationZone.Value);
                    var maxRadius = GetRB(value.Power, value.Gain, value.TransmitLossFactor, dbRaz);
                    BiohazardRadius biohazardRadius = new BiohazardRadius()
                    {
                        Degree = radiationZone.Degree,
                        Dbi = radiationZone.Value,
                        DbiRaz = dbRaz,
                        MaximumBiohazardRadius = maxRadius,
                        BiohazardRadiusX = GetRX(radiationZone.Degree, maxRadius),
                        BiohazardRadiusZ = GetRZ(radiationZone.Degree, maxRadius)
                    };
                    await _repositoryWrapper.BiohazardRadiusRepository.CreateAsync(biohazardRadius);
                }
                
            }
            
        }
        await _repositoryWrapper.Save();
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Рассчеты произведены успешно успешно считан" },
            Success: true);
    }
    public decimal GetRB(decimal power,decimal height,decimal lost,decimal multiplier) //Rb,m
    {
        double rB = Math.Sqrt(8 * (double)power * (double)Multiplier(height) * (double)Multiplier(-lost) / 10) * 1 * (double)Multiplier(multiplier);
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
    public decimal Multiplier(decimal value)
    {
        return (decimal)Math.Pow(10, (double)value / 10);
    }
}