using System.Text.Json.Serialization;

namespace Application.Models.Antennas;

public class CreateProjectAntennaDto
{
    [property: JsonPropertyName("house")]
    public string House { get; set; }
}