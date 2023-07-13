using Domain.Common;

namespace Application.Models;

public class DistrictDTO : BaseEntity 
{
    public string DistrictName { get; set; }
    public List<TownDTO> Towns { get; set; }
}