using System.Text.Json.Serialization;

namespace Application.Models.ContrAgents;

public class CreateContrAgentDto
{
    [property: JsonPropertyName("companyName")] 
    public string CompanyName { get; set; }
    [property: JsonPropertyName("bin")] 
    public string BIN { get; set; }
    [property: JsonPropertyName("directorName")] 
    public string DirectorName { get; set; }
    [property: JsonPropertyName("directorSurname")] 
    public string DirectorSurname { get; set; }
    [property: JsonPropertyName("directorPatronymic")] 
    public string? DirectorPatronymic { get; set; }
    [property: JsonPropertyName("address")] 
    public string Address { get; set; }
    [property: JsonPropertyName("email")]
    public string? Email { get; set; }
    [property: JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }
}