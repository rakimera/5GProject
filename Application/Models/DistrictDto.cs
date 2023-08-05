using Domain.Common;

namespace Application.Models;

public class DistrictDto : BaseEntity 
{
    public string DistrictName { get; set; }
    public List<TownDto> Towns { get; set; }
}