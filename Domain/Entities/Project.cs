using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity
{
    public int ContrAgentId { get; set; }
    public ContrAgent ContrAgent { get; set; }
    public int ExecutorId { get; set; }
    public User Executor { get; set; }
    public int ProjectStatusId { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public int DistrictId { get; set; }
    public District District { get; set; }
    public int TownId { get; set; }
    public Town Town { get; set; }
    public string? Street { get; set; }
    public string? House { get; set; }
    public List<ProjectAntenna> ProjectAntennae { get; set; } = new List<ProjectAntenna>();
}