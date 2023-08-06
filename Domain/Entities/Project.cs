using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity
{
    public Guid ContrAgentId { get; set; }
    public ContrAgent ContrAgent { get; set; }
    public Guid ExecutorId { get; set; }
    public User Executor { get; set; }
    public Guid ProjectStatusId { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public Guid DistrictId { get; set; }
    public District District { get; set; }
    public Guid TownId { get; set; }
    public Town Town { get; set; }
    public string? Street { get; set; }
    public string? House { get; set; }
    public List<ProjectAntenna> ProjectAntennae { get; set; } = new List<ProjectAntenna>();
}