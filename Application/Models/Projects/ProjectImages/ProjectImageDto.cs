
namespace Application.Models.Projects.ProjectImages;

public class ProjectImageDto
{
    public Guid Id { get; set; }
    public required string Route { get; set; }
    public Guid ProjectId { get; set; }
}