using System.Text.Json.Serialization;
using Domain.Common;

namespace Application.Models.RadiationZone.RadiationZoneExelFile;

public class RadiationZoneExelFileDto : BaseEntity
{
    [property: JsonPropertyName("exel")] 
    public byte[]? ExelFile { get; set; }

    [property: JsonPropertyName("translatorSpecId")]
    public Guid TranslatorSpecId { get; set; }
}