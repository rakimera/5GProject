using System.Text.Json.Serialization;
using Application.Models.Antennae;
using Application.Models.ContrAgents;
using Application.Models.Users;
using Domain.Common;
using Domain.Entities;

namespace Application.Models.Projects;

public class ProjectDto : BaseEntity
{
    [property: JsonPropertyName("contrAgentId")] 
    public required string ContrAgentId { get; set; }
    public ContrAgentDto ContrAgent { get; set; }
    
    [property: JsonPropertyName("executorId")]
    public string ExecutorId { get; set; }
    public UserDto Executor { get; set; }
    
    [property: JsonPropertyName("projectStatusId")]
    public string ProjectStatusId { get; set; }
    
    [property: JsonPropertyName("projectStatus")]
    public ProjectStatusDto ProjectStatus { get; set; }
    
    [property: JsonPropertyName("projectNumber")]
    public int ProjectNumber { get; set; }
    
    [property: JsonPropertyName("executiveCompanyId")]
    public string ExecutiveCompanyId { get; set; }
    
    [property: JsonPropertyName("executiveCompany")]
    public ExecutiveCompany ExecutiveCompany { get; set; }
    
    [property: JsonPropertyName("townName")] 
    public required string TownName { get; set; }

    [property: JsonPropertyName("arial")] 
    public string? Arial { get; set; }
    
    [property: JsonPropertyName("street")] 
    public string? Street { get; set; }
    
    [property: JsonPropertyName("house")] 
    public string? House { get; set; }
    public List<ProjectAntennaDto> ProjectAntennae { get; set; } = new List<ProjectAntennaDto>();
}