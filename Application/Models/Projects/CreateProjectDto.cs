using System.Text.Json.Serialization;
using Application.Models.Address;

namespace Application.Models.Projects;

public class CreateProjectDto
{
    [property: JsonPropertyName("contrAgentId")] 
    public required string ContrAgentId { get; set; }
    
    [property: JsonPropertyName("townName")] 
    public required string TownName { get; set; }

    [property: JsonPropertyName("arial")] 
    public string? Arial { get; set; }
    
    [property: JsonPropertyName("street")] 
    public string? Street { get; set; }
    
    [property: JsonPropertyName("house")] 
    public string? House { get; set; }
    
    [property: JsonPropertyName("projectNumber")] 
    public int ProjectNumber { get; set; }
}