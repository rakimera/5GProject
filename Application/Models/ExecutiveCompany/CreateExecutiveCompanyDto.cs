namespace Application.Models.ExecutiveCompany;

public class CreateExecutiveCompanyDto
{
    public string Address { get; set; }
    public string LicenseNumber { get; set; }
    public DateTime LicenseDateOfIssue { get; set; }
    public string BIN { get; set; }
    public string CompanyName { get; set; }
}