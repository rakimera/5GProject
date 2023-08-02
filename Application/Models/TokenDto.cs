using System.Text.Json.Serialization;

namespace Application.Models;

public class TokenDto
{
    [property: JsonPropertyName("accessToken")] 
    public string? AccessToken { get; set; }
    
    [property: JsonPropertyName("refreshToken")] 
    public string? RefreshToken { get; set; }
}