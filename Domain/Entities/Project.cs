using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity
{
    public string ContrAgentOid { get; set; }
    public ContrAgent ContrAgent { get; set; }
    public string ExecutorOid { get; set; }
    public User Executor { get; set; }
    public string ProjectStatusOid { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public string DistrictOid { get; set; }
    public District District { get; set; }
    public string TownOid { get; set; }
    public Town Town { get; set; }
    public string? Street { get; set; }
    public string? House { get; set; }
    public List<ProjectAntenna> ProjectAntennae { get; set; } = new List<ProjectAntenna>();
}