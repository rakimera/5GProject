using Domain.Common;

namespace Domain.Entities;

public class ExecutiveCompany : BaseEntity
{
    public required string Address { get; set; }
    public string? LicenseNumber { get; set; }
    public DateTime? LicenseDateOfIssue { get; set; }
    public string BIN { get; set; }
    public string CompanyName { get; set; }
}