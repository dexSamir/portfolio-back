using System.Net;

namespace Portfolio.Application.Exceptions;

public abstract class BaseException(string message, HttpStatusCode statusCode, string? errorCode = null, int code = 0)
    : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public int Code { get; } = code;
    public string? ErrorCode { get; } = errorCode;
}