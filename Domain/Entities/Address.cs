using Domain.Common;

namespace Domain.Entities;

public class Address : BaseEntity
{
    public Guid TownId { get; set; }
    public Town Town { get; set; }
    public string Arial { get; set; }
    public string Street { get; set; }
    public string House { get; set; }
    public string Flat { get; set; }
}