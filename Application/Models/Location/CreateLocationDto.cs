namespace Application.Models.Location;

public class CreateLocationDto
{
    public decimal Tilt { get; set; }
    public decimal Height { get; set; }
    public decimal Azimuth { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}