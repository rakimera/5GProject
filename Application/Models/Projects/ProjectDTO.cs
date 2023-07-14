using Application.Models.Antennas;
using Application.Models.Users;
using Domain.Common;

namespace Application.Models.Projects;

public class ProjectDTO : BaseEntity
{
    public Guid ContrAgentId { get; set; }
    public ContrAgentDTO ContrAgent { get; set; }
    public Guid ExecutorId { get; set; }
    public UserDTO Executor { get; set; }
    public Guid ProjectStatusId { get; set; }
    public ProjectStatusDTO ProjectStatus { get; set; }
    public Guid DistrictId { get; set; }
    public DistrictDTO District { get; set; }
    public Guid TownId { get; set; }
    public TownDTO Town { get; set; }
    public string? Street { get; set; }
    public string? House { get; set; }
    public List<ProjectAntennaDTO> ProjectAntennae { get; set; } = new List<ProjectAntennaDTO>();
}