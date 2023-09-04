
using Domain.Common;

namespace Application.Models.Projects.ProjectImages;

public class ProjectImageDto : BaseEntity
{
    public required string Route { get; set; }
    public Guid ProjectId { get; set; }
}