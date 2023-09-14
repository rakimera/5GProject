using Domain.Common;
using Domain.Enums;

namespace Application.Models.RadiationZone;

public class RadiationZoneDto : BaseEntity
{
    public int Degree { get; set; }
    public decimal Value { get; set; }
    public DirectionType DirectionType { get; set; }
    public Guid TranslatorSpecsId  { get; set; }
}