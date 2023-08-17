namespace Application.Models.Address;

public class AddressDto
{
    public string TownId { get; set; }
    public TownDto Town { get; set; }
    public string Arial { get; set; }
    public string Street { get; set; }
    public string House { get; set; }
    public string Flat { get; set; }
}