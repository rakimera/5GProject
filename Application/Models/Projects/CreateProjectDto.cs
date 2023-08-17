using System.Text.Json.Serialization;
using Application.Models.Address;
using Application.Models.Antennas;

namespace Application.Models.Projects;

public class CreateProjectDto
{
    [property: JsonPropertyName("contrAgentId")] 
    public string ContrAgentId { get; set; }

    [property: JsonPropertyName("address")]
    public CreateAddressDto Address { get; set; }
    
    [property: JsonPropertyName("projectAntennas")]
    public List<CreateProjectAntennaDto> CreateProjectAntennas { get; set; }
}