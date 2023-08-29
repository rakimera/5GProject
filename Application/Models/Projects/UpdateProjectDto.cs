using System.Text.Json.Serialization;
using Domain.Common;

namespace Application.Models.Projects;

public class UpdateProjectDto : BaseEntity
{
    [property: JsonPropertyName("projectNumber")]
    public required string ProjectNumber { get; set; }
    
    [property: JsonPropertyName("contrAgentId")] 
    public required string ContrAgentId { get; set; }

    [property: JsonPropertyName("executorId")]
    public string ExecutorId { get; set; }

    [property: JsonPropertyName("executiveCompanyId")]
    public string ExecutiveCompanyId { get; set; }

    [property: JsonPropertyName("projectStatusId")]
    public string ProjectStatusId { get; set; }

    [property: JsonPropertyName("townName")] 
    public required string TownName { get; set; }
    [property: JsonPropertyName("districtName")] 
    public string? DistrictName { get; set; }

    [property: JsonPropertyName("address")] 
    public string? Address { get; set; }
}