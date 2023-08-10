using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity
{
    public Guid ContrAgentId { get; set; }
    public CounterAgent CounterAgent { get; set; }
    public Guid ExecutorId { get; set; }
    public User Executor { get; set; }
    public SanPinDock? SanPinDock { get; set; }
    public DateTime? YearOfInitial { get; set; }
    public Guid ProjectStatusId { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public Address Address { get; set; }
    public Guid AddressId { get; set; }
    public List<ProjectAntenna> ProjectAntennae { get; set; } = new List<ProjectAntenna>();
    public List<TotalFluxDensity> TotalFluxDensity { get; set; } = new List<TotalFluxDensity>();
}