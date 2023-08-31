using System.Text.Json.Serialization;
using Application.Models.Antennae;
using Application.Models.RadiationZone;
using Domain.Common;

namespace Application.Models.TranslatorSpecs;

public class TranslatorSpecsDto : BaseEntity
{
    [property: JsonPropertyName("frequency")]
    public decimal Frequency { get; set; }
    public decimal Power { get; set; }
    public decimal Gain { get; set; }
    public Guid AntennaId { get; set; }
    public AntennaDto Antenna { get; set; }
    public List<RadiationZoneDto> RadiationZones { get; set; }
}