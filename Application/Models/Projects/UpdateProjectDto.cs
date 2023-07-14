using Domain.Common;

namespace Application.Models.Projects;

public class UpdateProjectDto
{
    public Guid Oid { get; set; }
    public Guid ProjectStatusId { get; set; }
}