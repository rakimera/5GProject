using System.Text.Json.Serialization;
using Application.Models.Projects.ProjectAntennas;
using Application.Models.TranslatorSpecs;
using Domain.Common;

namespace Application.Models.AntennaTranslator;

public class AntennaTranslatorDto : BaseEntity
{
    [property: JsonPropertyName("projectAntennaId")]
    public Guid ProjectAntennaId { get; set; }
    
    [property: JsonPropertyName("projectAntenna")]
    public ProjectAntennaDto? ProjectAntenna { get; set; }
    
    [property: JsonPropertyName("translatorSpecsId")]
    public Guid TranslatorSpecsId { get; set; }
    
    [property: JsonPropertyName("translatorSpecs")]
    public TranslatorSpecsDto? TranslatorSpecs { get; set; }
    
    [property: JsonPropertyName("power")]
    public decimal Power { get; set; }
    
    [property: JsonPropertyName("translatorTypeId")]
    public Guid TranslatorTypeId { get; set; }
    
    [property: JsonPropertyName("translatorType")]
    public TranslatorTypeDto? TranslatorType { get; set; }
    
    [property: JsonPropertyName("gain")]
    public decimal Gain { get; set; }
    
}