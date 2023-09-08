using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity
{
    public required string ProjectNumber { get; set; }
    public Guid ContrAgentId { get; set; }
    public ContrAgent ContrAgent { get; set; }
    public Guid ExecutorId { get; set; }
    public User Executor { get; set; }
    public Guid? ExecutiveCompanyId { get; set; }
    public string? PurposeRto { get; set; }
    public string? PlaceOfInstall { get; set; }
    public string? MaxHeightAdjoinBuild { get; set; }
    public string? PurposeBuild { get; set; }
    public string? TypeORoof { get; set; }
    public string? TypeOfTopCover { get; set; }
    public string? PlaceOfCommunicationCloset { get; set; }
    public bool? HasTechnicalLevel { get; set; }
    public bool? HasOtherRto { get; set; }
    public ExecutiveCompany ExecutiveCompany { get; set; }
    public Guid? SanPinDockId { get; set; }
    public SanPinDock? SanPinDock { get; set; }
    public DateTime? YearOfInitial { get; set; }
    public Guid ProjectStatusId { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public Guid? SummaryBiohazardRadiusId { get; set; }
    public SummaryBiohazardRadius SummaryBiohazardRadius { get; set; }
    public required string DistrictName { get; set; }
    public required string TownName { get; set; }
    public string? Address { get; set; }
    public List<ProjectAntenna> ProjectAntennae { get; set; } = new List<ProjectAntenna>();
    public List<TotalFluxDensity> TotalFluxDensity { get; set; } = new List<TotalFluxDensity>();
}