using Domain.Common;

namespace Domain.Entities;

public class ExecutiveCompany : BaseEntity
{
    public required string Address { get; set; }
    public string? LicenseNumber { get; set; }
    public DateTime? LicenseDateOfIssue { get; set; }
    public required string BIN { get; set; }
    public required string CompanyName { get; set; }
    public required string DirectorName { get; set; }
    public required string DirectorSurname { get; set; }
    public string? DirectorPatronymic { get; set; }
    public required string TownName { get; set; }
}