using Domain.Common;

namespace Domain.Entities;

public class AntennaTranslator : BaseEntity
{
    public Guid TranslatorSpecsId { get; set; }
    public TranslatorSpecs TranslatorSpecs { get; set; }
    public decimal Power { get; set; }
    public decimal TransmitLossFactor { get; set; }
    public Guid? TranslatorTypeId { get; set; }
    public TranslatorType TranslatorType { get; set; }
    public decimal Gain { get; set; }
    public decimal Tilt { get; set; }
    public Guid ProjectAntennaId { get; set; }
    public ProjectAntenna ProjectAntenna { get; set; }
    public List<EnergyResult> EnergyResults { get; set; } = new List<EnergyResult>();
    public List<BiohazardRadius> BiohazardRadii { get; set; } = new List<BiohazardRadius>();
}