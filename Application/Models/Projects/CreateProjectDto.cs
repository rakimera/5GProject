using System.Text.Json.Serialization;
using Application.Models.Address;

namespace Application.Models.Projects;

public class CreateProjectDto
{
    [property: JsonPropertyName("contrAgentId")] 
    public required string ContrAgentId { get; set; }
    
    [property: JsonPropertyName("townName")] 
    public required string TownName { get; set; }

    [property: JsonPropertyName("address")] 
    public string? Address { get; set; }

    [property: JsonPropertyName("projectNumber")] 
    public required string ProjectNumber { get; set; }
}