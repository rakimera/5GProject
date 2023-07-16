namespace Application.Models.Projects;

public class CreateProjectDto
{
    public string ContrAgentId { get; set; }
    public string ExecutorId { get; set; }
    public string ProjectStatusId { get; set; }
    public string DistrictId { get; set; }
    public string TownId { get; set; }
    public string? Street { get; set; }
    public string? House { get; set; }
}