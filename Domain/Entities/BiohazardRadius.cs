using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class BiohazardRadius : BaseEntity
{
    public int Degree { get; set; }
    public decimal MaximumBiohazardRadius { get; set; }
    public decimal BiohazardRadiusZ { get; set; }
    public decimal BiohazardRadiusX { get; set; }
    public DirectionType DirectionType { get; set; }
    public Guid AntennaTranslatorId  { get; set; }
    public AntennaTranslator AntennaTranslator { get; set; }
}