using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class RadiationZone : BaseEntity
{
    public int Degree { get; set; }
    public decimal Value { get; set; }
    public DirectionType DirectionType { get; set; }
    public Guid TranslatorSpecsId  { get; set; }
    public TranslatorSpecs TranslatorSpecs { get; set; }
}