namespace Portfolio.Application.Exceptions.Common;

using System.Net;
public class UnsupportedDeleteTypeException(string message, string? errorCode = null, int code = 0)
    : BaseException(message, HttpStatusCode.BadRequest, errorCode, code);