using Domain.Common;

namespace Domain.Entities;

public class Antenna : BaseEntity
{
    public required string Model { get; set; }
    public decimal VerticalSizeDiameter { get; set; }
    public List<TranslatorSpecs> TranslatorSpecsList { get; set; } = new List<TranslatorSpecs>();
}