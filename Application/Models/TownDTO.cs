using Domain.Common;

namespace Application.Models;

public class TownDTO : BaseEntity
{
    public string TownName { get; set; }
    public int DistrictId { get; set; }
    public DistrictDTO District{ get; set; }
}