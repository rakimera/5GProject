using System.Text.Json.Serialization;

namespace Application.Models.Users;

public class UpdateUserDto
{
    [property: JsonPropertyName("id")]
    public string Id { get; set; }
    
    [property: JsonPropertyName("login")]
    public string Login { get; set; }
    
    [property: JsonPropertyName("name")]
    public string Name { get; set; }
    
    [property: JsonPropertyName("surname")]
    public string Surname { get; set; }
    
    [property: JsonPropertyName("roles")]
    public List<string> Roles { get; set; }
    
    [property: JsonPropertyName("executiveCompanyId")]
    public Guid? ExecutiveCompanyId { get; set; }
    
    [property: JsonPropertyName("patronymic")]
    public string? Patronymic { get; set; }
    
    [property: JsonPropertyName("phoneNumber")]
    public required string PhoneNumber { get; set; }
    
    [property: JsonPropertyName("password")]
    public string? Password { get; set; }
}