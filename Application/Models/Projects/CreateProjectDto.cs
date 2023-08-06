using System.Text.Json.Serialization;

namespace Application.Models.Projects;

public class CreateProjectDto
{
    [property: JsonPropertyName("contrAgentId")] 
    public string ContrAgentId { get; set; }
    [property: JsonPropertyName("districtId")] 
    public string DistrictId { get; set; }
    [property: JsonPropertyName("townId")] 
    public string TownId { get; set; }
    [property: JsonPropertyName("street")] 
    public string? Street { get; set; }
    [property: JsonPropertyName("house")] 
    public string? House { get; set; }
}