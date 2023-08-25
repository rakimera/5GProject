using Application.Models.TranslatorSpecs;
using Domain.Enums;

namespace Application.Models.RadiationZone;

public class UpdateRadiationZoneDto
{
    public string Id { get; set; }
    public int Degree { get; set; }
    public decimal Value { get; set; }
    public DirectionType DirectionType { get; set; }
    public Guid TranslatorSpecsId  { get; set; }
    public TranslatorSpecsDto TranslatorSpecs { get; set; }
}