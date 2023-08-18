using System.Text.Json.Serialization;
using Domain.Common;

namespace Application.Models.CompanyLicense;

public class CompanyLicenseDto : BaseEntity
{
    [property: JsonPropertyName("number")] public string Number { get; set; }

    [property: JsonPropertyName("dateOfIssue")]
    public DateTime DateOfIssue { get; set; }
}