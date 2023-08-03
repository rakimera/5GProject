using System.Text.Json.Serialization;

namespace Application.Models.ContrAgents;

public class CreateContrAgentDto
{
    [property: JsonPropertyName("companyName")] 
    public string CompanyName { get; set; }
    [property: JsonPropertyName("bin")] 
    public int BIN { get; set; }
    [property: JsonPropertyName("directorName")] 
    public string DirectorName { get; set; }
    [property: JsonPropertyName("directorSurname")] 
    public string DirectorSurname { get; set; }
    [property: JsonPropertyName("directorPatronymic")] 
    public string DirectorPatronymic { get; set; }
    [property: JsonPropertyName("amplificationFactor")] 
    public decimal AmplificationFactor { get; set; }
}