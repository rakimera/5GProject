using Domain.Common;

namespace Domain.Entities;

public class TranslatorSpecs : BaseEntity 
{ 
    public int AntennaId { get; set; }
    public Antenna Antenna { get; set; }
}