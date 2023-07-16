using Domain.Common;

namespace Application.Models.Projects;

public class ProjectStatusDto : BaseEntity
{
    public required string Status { get; set; }
}