using Application.Models.Antennae;
using Application.Models.RadiationZone;
using Microsoft.AspNetCore.Http;

namespace Application.Models.TranslatorSpecs;

public class CreateTranslatorSpecsDto
{
    public decimal Frequency { get; set; }
    public Guid? AntennaId { get; set; }
    public AntennaDto? Antenna { get; set; }
    public List<RadiationZoneDto>? RadiationZones { get; set; }
}