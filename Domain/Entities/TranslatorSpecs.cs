using Domain.Common;

namespace Domain.Entities;

public class TranslatorSpecs : BaseEntity 
{
    public decimal Frequency { get; set; }
    public decimal Power { get; set; }
    public decimal Gain { get; set; }
    public Guid AntennaId { get; set; }
    public Antenna Antenna { get; set; }
    public List<RadiationZone> RadiationZones { get; set; }
}