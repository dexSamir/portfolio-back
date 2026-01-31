namespace Portfolio.Application.Exceptions.Common;

using System.Net;
public class NotFoundException(string message, string? errorCode = null, int code = 0)
    : BaseException(message, HttpStatusCode.NotFound, errorCode, code);

public class NotFoundException<T>(string? errorCode = null, int code = 0)
    : NotFoundException($"{typeof(T).Name} is not found", errorCode, code);
