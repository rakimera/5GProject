
using Domain.Common;

namespace Application.Models.Projects.ProjectImages;

public class ProjectImageDto : BaseEntity
{
    public string? Route { get; set; }
    public Guid ProjectId { get; set; }
}