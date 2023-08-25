using System.Text.Json.Serialization;
using Application.Models.TranslatorSpecs;
using Domain.Common;

namespace Application.Models.Antennae;

public class AntennaDto : BaseEntity
{
    [property: JsonPropertyName("model")]
    public required string Model { get; set; }
    public decimal VerticalSizeDiameter { get; set; }
    public List<TranslatorSpecsDto> TranslatorSpecsList { get; set; } = new List<TranslatorSpecsDto>();
}