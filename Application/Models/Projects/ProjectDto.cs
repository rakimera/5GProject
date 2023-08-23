using System.Text.Json.Serialization;
using Application.Models.Antennae;
using Application.Models.ContrAgents;
using Application.Models.ExecutiveCompany;
using Application.Models.Users;
using Domain.Common;
using Domain.Entities;

namespace Application.Models.Projects;

public class ProjectDto : BaseEntity
{
    [property: JsonPropertyName("projectNumber")]
    public required string ProjectNumber { get; set; }
    
    [property: JsonPropertyName("contrAgentId")] 
    public required string ContrAgentId { get; set; }
    public ContrAgentDto ContrAgent { get; set; }
    
    [property: JsonPropertyName("executorId")]
    public string ExecutorId { get; set; }
    public UserDto Executor { get; set; }
    
    [property: JsonPropertyName("executiveCompanyId")]
    public string ExecutiveCompanyId { get; set; }
    
    [property: JsonPropertyName("executiveCompany")]
    public ExecutiveCompanyDto ExecutiveCompany { get; set; }
    
    [property: JsonPropertyName("projectStatusId")]
    public string ProjectStatusId { get; set; }

    [property: JsonPropertyName("townName")] 
    public required string TownName { get; set; }
    
    [property: JsonPropertyName("districtName")] 
    public string? DistrictName { get; set; }

    [property: JsonPropertyName("address")] 
    public string? Address { get; set; }
    
    public List<ProjectAntennaDto> ProjectAntennae { get; set; } = new List<ProjectAntennaDto>();
}