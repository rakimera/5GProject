using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Application.Models.TranslatorSpecs;

public class CreateTranslatorSpecsDto
{
    [property: JsonPropertyName("frequency")] 
    public decimal Frequency { get; set; }
    
    [property: JsonPropertyName("antennaId")]
    public Guid? AntennaId { get; set; }
    
    [property: JsonPropertyName("vertical")]
    public IFormFile Vertical { get; set; }
    
    [property: JsonPropertyName("horizontal")]
    public IFormFile Horizontal { get; set; }
}