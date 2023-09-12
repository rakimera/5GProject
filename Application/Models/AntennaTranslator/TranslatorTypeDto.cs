using System.Text.Json.Serialization;
using Domain.Common;

namespace Application.Models.AntennaTranslator;

public class TranslatorTypeDto : BaseEntity
{
    [property: JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [property: JsonPropertyName("type")]
    public required string Type { get; set; }
}