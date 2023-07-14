using Domain.Common;

namespace Domain.Entities;

public class Town : BaseEntity
{
    public string TownName { get; set; }
    public string DistrictOid { get; set; }
    public District District{ get; set; }
}