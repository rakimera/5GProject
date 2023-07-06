using Domain.Common;

namespace Domain.Entities;

public class ProjectAntenna : BaseEntity
{
    public int LocationId { get; set; }
    public Location Location { get; set; }
    public int AntennaId { get; set; }
    public Antenna Antenna { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}