using Domain.Common;

namespace Domain.Entities;

public class EnergyResult : BaseEntity
{
    public decimal Distance { get; set; }
    public decimal Value { get; set; }
    public Guid AntennaTranslatorId { get; set; }
    public AntennaTranslator AntennaTranslator { get; set; }
}