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
    
    [property: JsonPropertyName("purposeRto")] 
    public string? PurposeRto { get; set; }
    
    [property: JsonPropertyName("placeOfInstall")] 
    public string? PlaceOfInstall { get; set; }
    
    [property: JsonPropertyName("maxHeightAdjoinBuild")] 
    public decimal? MaxHeightAdjoinBuild { get; set; }
    
    [property: JsonPropertyName("purposeBuild")] 
    public string? PurposeBuild { get; set; }
    
    [property: JsonPropertyName("typeORoof")] 
    public string? TypeORoof { get; set; }
    
    [property: JsonPropertyName("typeOfTopCover")] 
    public string? TypeOfTopCover { get; set; }
    
    [property: JsonPropertyName("placeOfCommunicationCloset")] 
    public string? PlaceOfCommunicationCloset { get; set; }
    
    [property: JsonPropertyName("hasTechnicalLevel")] 
    public bool? HasTechnicalLevel { get; set; }
    
    [property: JsonPropertyName("hasOtherRto")] 
    public bool? HasOtherRto { get; set; }
}