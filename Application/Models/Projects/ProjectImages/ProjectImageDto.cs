
using System.Text.Json.Serialization;
using Domain.Common;

namespace Application.Models.Projects.ProjectImages;

public class ProjectImageDto : BaseEntity
{
    [property: JsonPropertyName("image")] 
    public byte[]? Image { get; set; }

    [property: JsonPropertyName("projectId")]
    public Guid ProjectId { get; set; }
}