using Domain.Common;

namespace Domain.Entities;

public class EnergyResult : BaseEntity
{
    public decimal _5 { get; set; }
    public decimal _10 { get; set; }
    public decimal _20 { get; set; }
    public decimal _30 { get; set; }
    public decimal _40 { get; set; }
    public decimal _60 { get; set; }
    public decimal _80 { get; set; }
    public decimal _100 { get; set; }
    public string TranslatorSpecsOid { get; set; }
    public TranslatorSpecs TranslatorSpecs { get; set; }
}