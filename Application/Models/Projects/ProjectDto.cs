using Application.Models.Antennae;
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
    public int ProjectNumber { get; set; }
    public ProjectStatusDto ProjectStatus { get; set; }
    public string ExecutiveCompanyId { get; set; }
    public ExecutiveCompany ExecutiveCompany { get; set; }
    public string Address { get; set; }
    public List<ProjectAntennaDto> ProjectAntennae { get; set; } = new List<ProjectAntennaDto>();
}