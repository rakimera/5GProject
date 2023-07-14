using Domain.Common;

namespace Domain.Entities;

public class ProjectAntenna : BaseEntity
{
    public string LocationOid { get; set; }
    public Location Location { get; set; }
    public string AntennaOid { get; set; }
    public Antenna Antenna { get; set; }
    public string ProjectOid { get; set; }
    public Project Project { get; set; }
}