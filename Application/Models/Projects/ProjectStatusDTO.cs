using Domain.Common;

namespace Application.Models.Projects;

public class ProjectStatusDTO : BaseEntity
{
    public required string Status { get; set; }
}