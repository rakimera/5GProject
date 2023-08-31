using System.Text.Json.Serialization;
using Application.Models.AntennaTranslator;
using Application.Models.Location;

namespace Application.Models.Projects.ProjectAntennas;

public class CreateProjectAntennaDto
{
    [property: JsonPropertyName("azimuth")]
    public decimal Azimuth { get; set; }
    
    [property: JsonPropertyName("height")]
    public decimal Height { get; set; }
    
    [property: JsonPropertyName("latitude")]
    public decimal Latitude { get; set; }
    
    [property: JsonPropertyName("longitude")]
    public decimal Longitude { get; set; }
    
    [property: JsonPropertyName("tilt")]
    public decimal Tilt { get; set; }
    
    [property: JsonPropertyName("antennaId")]
    public Guid AntennaId { get; set; }

    [property: JsonPropertyName("projectId")]
    public Guid ProjectId { get; set; }
}