using Domain.Common;

namespace Domain.Entities;

public class ProjectAntenna : BaseEntity
{
    public Guid LocationId { get; set; }
    public Location Location { get; set; }
    public Guid AntennaId { get; set; }
    public Antenna Antenna { get; set; }
    public Guid ProjectOId { get; set; }
    public Project Project { get; set; }
}