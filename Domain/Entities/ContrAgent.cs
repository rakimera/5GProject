using Domain.Common;

namespace Domain.Entities;

public class ContrAgent : BaseEntity
{
    public required string CompanyName { get; set; }
    public required string BIN { get; set; }
    public required string DirectorName { get; set; }
    public required string DirectorSurname { get; set; }
    public required string DirectorPatronymic { get; set; }
    public decimal TransmitLossFactor { get; set; }
    public required string Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}