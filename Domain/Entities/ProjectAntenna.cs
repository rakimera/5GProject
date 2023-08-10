using Domain.Common;

namespace Domain.Entities;

public class ProjectAntenna : BaseEntity
{
    public Guid LocationId { get; set; }
    public Location Location { get; set; }
    public Guid AntennaTranslatorId { get; set; }
    public AntennaTranslator AntennaTranslator { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
}  