namespace Application.DataObjects;

public record BaseResponse<T>(T Result, string Message); //вот его я предлагаю использовать для ответа на API обсудить