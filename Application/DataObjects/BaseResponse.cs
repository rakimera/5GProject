namespace Application.DataObjects;

public record BaseResponse<T>(T Result, string Message);