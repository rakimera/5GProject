using System.Text.Json.Serialization;
using Application.Models.TranslatorSpecs;

namespace Application.Models.Antennae;

public class CreateAntennaDto
{
    [property: JsonPropertyName("model")] 
    public string Model { get; set; }
    [property: JsonPropertyName("verticalSizeDiameter")] 
    public decimal VerticalSizeDiameter { get; set; }
    [property: JsonPropertyName("translatorSpecsList")] 
    public List<TranslatorSpecsDto>? TranslatorSpecsList { get; set; } = new List<TranslatorSpecsDto>();
}