using System.Text.Json.Serialization;

namespace Application.Models.AntennaTranslator;

public class TranslatorTypeDto
{
    [property: JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [property: JsonPropertyName("type")]
    public required string Type { get; set; }
}