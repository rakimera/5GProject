using System.Text.Json.Serialization;

namespace Application.Models.ExecutiveCompany;

public class CreateExecutiveCompanyDto
{
    [property: JsonPropertyName("address")]
    public string Address { get; set; }
    [property: JsonPropertyName("licenseNumber")]
    public string? LicenseNumber { get; set; }
    [property: JsonPropertyName("licenseDateOfIssue")]
    public DateTime? LicenseDateOfIssue { get; set; }
    [property: JsonPropertyName("bin")]
    public string BIN { get; set; }
    [property: JsonPropertyName("companyName")]
    public string CompanyName { get; set; }
}