namespace Application.Models.Address;

public class CreateTownDto
{
    public required string TownName { get; set; }
    public required string DistrictId { get; set; }
}