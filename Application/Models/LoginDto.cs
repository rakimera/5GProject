using System.Text.Json.Serialization;

namespace Application.Models;

public class LoginDto
{
    [property: JsonPropertyName("login")] 
    public required string Login { get; set; }
    [property: JsonPropertyName("password")] 
    public required string Password { get; set; }
}