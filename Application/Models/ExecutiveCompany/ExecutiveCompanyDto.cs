using Application.Models.CompanyLicense;
using Application.Models.Projects;
using Application.Models.Users;
using Domain.Common;
using Domain.Entities;

namespace Application.Models.ExecutiveCompany;

public class ExecutiveCompanyDto : BaseEntity
{
    public required string Address { get; set; }
    public Guid CompanyLicenseId { get; set; }
    public CompanyLicenseDto? CompanyLicense { get; set; }
    public string BIN { get; set; }
    public string CompanyName { get; set; }
}