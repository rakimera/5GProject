using System.Text.Json.Serialization;
using Application.Models.Antennae;
using Application.Models.EnergyResult;
using Application.Models.TranslatorSpecs;

namespace Application.Models.AntennaTranslator;

public class AntennaTranslatorDto
{
    public Guid AntennaId { get; set; }
    public AntennaDto Antenna { get; set; }
    public Guid TranslatorSpecsId { get; set; }
    
    [property: JsonPropertyName("translatorSpecs")]
    public TranslatorSpecsDto? TranslatorSpecs { get; set; }
    public decimal Power { get; set; }
    public Guid TranslatorTypeId { get; set; }
    public TranslatorTypeDto TranslatorType { get; set; }
    public decimal Gain { get; set; }
    public List<EnergyResultDto> EnergyResults { get; set; } = new List<EnergyResultDto>();
}