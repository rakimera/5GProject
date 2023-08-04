using Domain.Common;

namespace Application.Models;

public class TownDto : BaseEntity
{
    public string TownName { get; set; }
    public string DistrictOid { get; set; }
    public DistrictDto District{ get; set; }
}