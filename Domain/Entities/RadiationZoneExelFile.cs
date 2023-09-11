using Domain.Common;

namespace Domain.Entities;

public class RadiationZoneExelFile : BaseEntity
{
    public required byte[] ExelFile { get; set; }
    public Guid TranslatorSpecId { get; set; }
    public TranslatorSpecs? translatorSpec { get; set; }
}