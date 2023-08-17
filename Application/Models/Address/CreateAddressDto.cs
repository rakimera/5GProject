using System.Text.Json.Serialization;

namespace Application.Models.Address;

public class CreateAddressDto
{
    [property: JsonPropertyName("townId")]
    public string TownId { get; set; }
    
    [property: JsonPropertyName("arial")]
    public string Arial { get; set; }
    
    [property: JsonPropertyName("street")]
    public string Street { get; set; }
    
    [property: JsonPropertyName("house")]
    public string House { get; set; }
    
    [property: JsonPropertyName("flat")]
    public string Flat { get; set; }
}