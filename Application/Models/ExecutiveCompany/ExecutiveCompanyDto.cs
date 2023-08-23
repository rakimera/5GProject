using Domain.Common;
using Domain.Entities;

namespace Application.Models.ExecutiveCompany;

public class ExecutiveCompanyDto : BaseEntity
{
    public required string Address { get; set; }
    public required string LicenseNumber { get; set; }
    public DateTime LicenseDateOfIssue { get; set; }
    public string BIN { get; set; }
    public string CompanyName { get; set; }
    public List<Project?> Projects { get; set; }
    public List<User?> Users { get; set; }
}