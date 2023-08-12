using System.Text.Json.Serialization;
using Application.Models.AntennaTranslator;
using Application.Models.Location;

namespace Application.Models.Antennas;

public class CreateProjectAntennaDto
{
    [property: JsonPropertyName("location")]
    public CreateLocationDto Location { get; set; }
    
    [property: JsonPropertyName("antennaTranslator")]
    public CreateAntennaTranslatorDto AntennaTranslator { get; set; }
}