using Application.Models.Antennae;
using Application.Models.RadiationZone;

namespace Application.Models.TranslatorSpecs;

public class UpdateTranslatorSpecsDto
{
    public string Id { get; set; }
    public decimal Frequency { get; set; }
    public decimal Power { get; set; }
    public decimal Gain { get; set; }
    public Guid AntennaId { get; set; }
    public AntennaDto? Antenna { get; set; }
    public List<RadiationZoneDto>? RadiationZones { get; set; }
}