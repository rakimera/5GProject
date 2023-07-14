using Domain.Common;

namespace Domain.Entities;

public class TranslatorSpecs : BaseEntity 
{ 
    public string AntennaOid { get; set; }
    public Antenna Antenna { get; set; }
}