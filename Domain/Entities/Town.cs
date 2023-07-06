using Domain.Common;

namespace Domain.Entities;

public class Town : BaseEntity
{
    public string TownName { get; set; }
    public List<District> Districts { get; set; }
}