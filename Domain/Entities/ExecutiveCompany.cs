using Domain.Common;

namespace Domain.Entities;

public class ExecutiveCompany : BaseEntity
{
    public Guid AddressId { get; set; }
    public Address Address { get; set; }
    public Guid CompanyLicenseId { get; set; }
    public CompanyLicense CompanyLicense { get; set; }
    public string BIN { get; set; }
    public string CompanyName { get; set; }
}