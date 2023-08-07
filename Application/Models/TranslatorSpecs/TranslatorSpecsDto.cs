using Application.Models.Antennas;
using Domain.Common;

namespace Application.Models.TranslatorSpecs;

public class TranslatorSpecsDto : BaseEntity
{
    public Guid AntennaId { get; set; }
    public AntennaDto Antenna { get; set; }
}