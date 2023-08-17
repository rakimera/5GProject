using Domain.Common;

namespace Domain.Entities;

public class TotalFluxDensity : BaseEntity //суммарная мощность по всем транляторам
{
    public int Distance { get; set; }
    public decimal Value { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
}