using Domain.Common;

namespace Domain.Entities;

public class CounterAgent : BaseEntity
{
    public string CompanyName { get; set; }
    public string BIN { get; set; }
    public string DirectorName { get; set; }
    public string DirectorSurname { get; set; }
    public string DirectorPatronymic { get; set; }
    public decimal TransmitLossFactor { get; set; }
    public Address Address { get; set; }
    public Guid? AddressId { get; set; }
    public List<CounterAgentPowerFrequency> CounterAgentPowerFrequencies { get; set; }
}