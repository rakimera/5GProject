using Application.Models.Antennas;
using Application.Models.Users;
using Domain.Common;

namespace Application.Models.Projects;

public class ProjectDto : BaseEntity
{
    public string ContrAgentId { get; set; }
    public ContrAgentDto ContrAgent { get; set; }
    public string ExecutorId { get; set; }
    public UserDto Executor { get; set; }
    public string ProjectStatusId { get; set; }
    public ProjectStatusDto ProjectStatus { get; set; }
    public string DistrictId { get; set; }
    public DistrictDTO District { get; set; }
    public string TownId { get; set; }
    public TownDTO Town { get; set; }
    public string? Street { get; set; }
    public string? House { get; set; }
    public List<ProjectAntennaDto> ProjectAntennae { get; set; } = new List<ProjectAntennaDto>();
}