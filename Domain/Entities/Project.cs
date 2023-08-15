using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity
{
    public int ProjectNumber { get; set; }
    public Guid ContrAgentId { get; set; }
    public ContrAgent ContrAgent { get; set; }
    public Guid ExecutorId { get; set; }
    public User Executor { get; set; }
    public Guid ExecutiveCompanyId { get; set; }
    public ExecutiveCompany ExecutiveCompany { get; set; }
    public Guid? SanPinDockId { get; set; }
    public SanPinDock? SanPinDock { get; set; }
    public DateTime? YearOfInitial { get; set; }
    public Guid ProjectStatusId { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public required string Address { get; set; }
    public List<ProjectAntenna> ProjectAntennae { get; set; } = new List<ProjectAntenna>();
    public List<TotalFluxDensity> TotalFluxDensity { get; set; } = new List<TotalFluxDensity>();
}