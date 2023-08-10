using Domain.Common;

namespace Domain.Entities;

public class CounterAgentPowerFrequency : BaseEntity
{
    public int Power { get; set; }    
    public decimal Frequency { get; set; }    
    public decimal TranslatorType { get; set; }
    public Guid CounterAgentId { get; set; }
    public CounterAgent CounterAgent { get; set; }
}