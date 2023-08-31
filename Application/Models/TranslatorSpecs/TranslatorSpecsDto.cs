using System.Text.Json.Serialization;
using Application.Models.Antennae;
using Application.Models.RadiationZone;
using Domain.Common;

namespace Application.Models.TranslatorSpecs;

public class TranslatorSpecsDto : BaseEntity
{
    [property: JsonPropertyName("frequency")]
    public decimal Frequency { get; set; }

    [property: JsonPropertyName("gain")]
    public decimal Gain { get; set; }
    
    [property: JsonPropertyName("antennaId")]
    public Guid AntennaId { get; set; }
    
    [property: JsonPropertyName("antenna")]
    public AntennaDto? Antenna { get; set; }
    public List<RadiationZoneDto>? RadiationZones { get; set; }
}