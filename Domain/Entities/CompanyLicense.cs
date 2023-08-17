using Domain.Common;

namespace Domain.Entities;

public class CompanyLicense : BaseEntity
{
    public required string Number { get; set; }
    public DateTime DateOfIssue { get; set; }
}