using System.Text.Json.Serialization;

namespace Application.Models.AntennaTranslator;

public class CreateAntennaTranslatorDto
{
    [property: JsonPropertyName("antennaId")]
    public required string AntennaId { get; set; }
    
    [property: JsonPropertyName("translatorId")]
    public required string TranslatorId { get; set; }
}