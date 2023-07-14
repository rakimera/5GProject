using Domain.Common;

namespace Application.Models.Projects;

public class CreateProjectDto
{
    public Guid ContrAgentId { get; set; }
    public Guid ExecutorId { get; set; }
    public Guid ProjectStatusId { get; set; }
    public Guid DistrictId { get; set; }
    public Guid TownId { get; set; }
    public string? Street { get; set; }
    public string? House { get; set; }
}