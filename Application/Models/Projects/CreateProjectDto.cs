using System.Text.Json.Serialization;
using Application.Models.Address;

namespace Application.Models.Projects;

public class CreateProjectDto
{
    [property: JsonPropertyName("contrAgentId")] 
    public string ContrAgentId { get; set; }

    [property: JsonPropertyName("address")]
    public CreateAddressDto Address { get; set; }
}