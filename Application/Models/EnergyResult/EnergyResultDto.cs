using Application.Models.AntennaTranslator;

namespace Application.Models.EnergyResult;

public class EnergyResultDto
{
    public decimal Distance { get; set; }
    public decimal Value { get; set; }
    public Guid AntennaTranslatorId { get; set; }
    public AntennaTranslatorDto AntennaTranslator { get; set; }
}