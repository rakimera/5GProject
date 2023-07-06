using Domain.Common;

namespace Domain.Entities;

public class District : BaseEntity
{
    public string DistrictName { get; set; }
    public int TownId { get; set; }
    public Town  Town { get; set; }
}