using System.Text.Json.Serialization;
using Domain.Common;
using Domain.Entities;

namespace Application.Models.ExecutiveCompany;

public class ExecutiveCompanyDto : BaseEntity
{
    [property: JsonPropertyName("address")]
    public required string Address { get; set; }
    
    [property: JsonPropertyName("licenseNumber")]
    public string? LicenseNumber { get; set; }
    
    [property: JsonPropertyName("licenseDateOfIssue")]
    public DateTime? LicenseDateOfIssue { get; set; }
    
    [property: JsonPropertyName("bin")]
    public required string BIN { get; set; }
    
    [property: JsonPropertyName("companyName")]
    public required string CompanyName { get; set; }
    
    [property: JsonPropertyName("townName")]
    public required string TownName { get; set; }
    
    [property: JsonPropertyName("projects")]
    public List<Project>? Projects { get; set; }
    
    [property: JsonPropertyName("users")]
    public List<User>? Users { get; set; }
    
    [property: JsonPropertyName("directorName")]
    public required string DirectorName { get; set; }
    
    [property: JsonPropertyName("directorSurname")]
    public required string DirectorSurname { get; set; }
    
    [property: JsonPropertyName("directorPatronymic")]
    public string? DirectorPatronymic { get; set; }
}