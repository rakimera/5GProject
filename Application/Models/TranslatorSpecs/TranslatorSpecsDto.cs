using Application.Models.Antennas;
using Application.Models.RadiationZone;

namespace Application.Models.TranslatorSpecs;

public class TranslatorSpecsDto
{
    public decimal Frequency { get; set; }
    public decimal Power { get; set; }
    public decimal Gain { get; set; }
    public Guid AntennaId { get; set; }
    public AntennaDto Antenna { get; set; }
    public List<RadiationZoneDto> RadiationZones { get; set; }
}