using Domain.Common;

namespace Domain.Entities;

public class District : BaseEntity
{
    public string DistrictName { get; set; }
    public List<Town> Towns { get; set; }
} 