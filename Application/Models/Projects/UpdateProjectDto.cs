using Domain.Common;

namespace Application.Models.Projects;

public class UpdateProjectDto : BaseEntity
{
    public Guid ProjectStatusId { get; set; }
}