using Application.Models.TranslatorSpecs;

namespace Application.Models.RadiationZone;

public class RadiationZoneDto
{
    public int Degree { get; set; }
    public decimal Value { get; set; }
    public Guid TranslatorSpecsId  { get; set; }
    public TranslatorSpecsDto TranslatorSpecs { get; set; }
}