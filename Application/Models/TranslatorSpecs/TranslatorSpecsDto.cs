using Application.Models.Antennae;
using Application.Models.RadiationZone;
using Domain.Common;

namespace Application.Models.TranslatorSpecs;

public class TranslatorSpecsDto : BaseEntity
{
    public decimal Frequency { get; set; }
    public Guid AntennaId { get; set; }
    public AntennaDto? Antenna { get; set; }
    public List<RadiationZoneDto>? RadiationZones { get; set; }
}