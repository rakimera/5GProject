using System.Text.Json.Serialization;

namespace Application.Models.Antennas;

public class CreateAntennaDto
{
    [property: JsonPropertyName("model")] 
    public string Model { get; set; }
    [property: JsonPropertyName("verticalSizeDiameter")] 
    public decimal VerticalSizeDiameter { get; set; }
}