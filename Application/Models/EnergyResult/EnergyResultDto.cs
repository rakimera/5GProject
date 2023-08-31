using Application.Models.AntennaTranslator;
using Domain.Common;

namespace Application.Models.EnergyResult;

public class EnergyResultDto : BaseEntity
{
    public decimal Distance { get; set; }
    public decimal Value { get; set; }
    public Guid AntennaTranslatorId { get; set; }
    public AntennaTranslatorDto? AntennaTranslator { get; set; }
}