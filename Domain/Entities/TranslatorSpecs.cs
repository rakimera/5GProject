using Domain.Common;

namespace Domain.Entities;

public class TranslatorSpecs : BaseEntity 
{
    public decimal Frequency { get; set; }
    public Guid AntennaId { get; set; }
    public Antenna Antenna { get; set; }
    public List<RadiationZone>? RadiationZones { get; set; } //360 это таблица
}