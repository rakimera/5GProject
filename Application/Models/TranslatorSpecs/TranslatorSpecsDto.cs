using Application.Models.Antennae;
using Domain.Common;

namespace Application.Models.TranslatorSpecs;

public class TranslatorSpecsDto : BaseEntity
{
    public Guid AntennaId { get; set; }
    public AntennaDto Antenna { get; set; }
}