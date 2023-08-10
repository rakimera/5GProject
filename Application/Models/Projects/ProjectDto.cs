using Application.Models.Antennas;
using Application.Models.ContrAgents;
using Application.Models.Users;
using Domain.Common;
using Domain.Entities;

namespace Application.Models.Projects;

public class ProjectDto : BaseEntity
{
    public string ContrAgentId { get; set; }
    public ContrAgentDto ContrAgent { get; set; }
    public string ExecutorId { get; set; }
    public UserDto Executor { get; set; }
    public string ProjectStatusId { get; set; }
    public string AddressId { get; set; }
    public ProjectStatusDto ProjectStatus { get; set; }
    public List<ProjectAntennaDto> ProjectAntennae { get; set; } = new List<ProjectAntennaDto>();
    public List<TotalFluxDensity> TotalFluxDensity { get; set; } = new List<TotalFluxDensity>();
}