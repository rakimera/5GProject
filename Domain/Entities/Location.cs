using Domain.Common;

namespace Domain.Entities;

public class Location : BaseEntity
{
    public decimal Azimuth { get; set; }
    public decimal Height { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal Tilt { get; set; }
}