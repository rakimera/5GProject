using Domain.Common;

namespace Domain.Entities;

public class AntennaTranslator : BaseEntity
{
    public Guid AntennaId { get; set; }
    public Antenna Antenna { get; set; }
    public Guid TranslatorSpecsId { get; set; }
    public TranslatorSpecs TranslatorSpecs { get; set; }
    public Guid ProjectAntennaId { get; set; }
    public ProjectAntenna ProjectAntenna { get; set; }
    public List<EnergyResult> EnergyResults { get; set; } = new List<EnergyResult>();
}