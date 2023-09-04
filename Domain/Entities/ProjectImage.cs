using Domain.Common;

namespace Domain.Entities;

public class ProjectImage : BaseEntity
{
    public required string Route { get; set; }
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
}