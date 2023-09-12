using Domain.Common;

namespace Domain.Entities;

public class ProjectAntenna : BaseEntity
{
    public decimal Azimuth { get; set; }
    public decimal HeightFromGroundLevel { get; set; }
    public decimal HeightFromRoofLevel { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal Tilt { get; set; }
    public string? RtoRadiationMode { get; set; }
    public Guid AntennaId { get; set; }
    public Antenna Antenna { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
}  