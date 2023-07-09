using System.Text.Json.Serialization;

namespace Application.DataObjects;

public record BaseResponse<T>(
    [property: JsonPropertyName("result")] T? Result,
    [property: JsonPropertyName("success")] bool Success,
    [property: JsonPropertyName("statusCode")] int StatusCode,
    [property: JsonPropertyName("message")] string? Message = null);