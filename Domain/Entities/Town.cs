using Domain.Common;

namespace Domain.Entities;

public class Town : BaseEntity
{
    public required string TownName { get; set; }
    public Guid DistrictId { get; set; }
    public District District{ get; set; }
}