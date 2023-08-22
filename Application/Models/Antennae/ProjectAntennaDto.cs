using System.Text.Json.Serialization;
using Application.Models.AntennaTranslator;
using Domain.Common;

namespace Application.Models.Antennae;

public class ProjectAntennaDto : BaseEntity
{
    [property: JsonPropertyName("projectId")] 
    public string? ProjectId { get; set; }
    
    [property: JsonPropertyName("antennaId")] 
    public string? AntennaId { get; set; }
    
    [property: JsonPropertyName("height")] 
    public decimal Height { get; set; }
    
    [property: JsonPropertyName("tilt")] 
    public decimal Tilt { get; set; }
    
    [property: JsonPropertyName("azimuth")] 
    public decimal Azimuth { get; set; }
    
    [property: JsonPropertyName("latitude")]
    public decimal Latitude { get; set; }
    
    [property: JsonPropertyName("longitude")]
    public decimal Longitude { get; set; }
    
    [property: JsonPropertyName("antennaTranslators")]
    public List<AntennaTranslatorDto>? AntennaTranslators { get; set; }
    
   
}