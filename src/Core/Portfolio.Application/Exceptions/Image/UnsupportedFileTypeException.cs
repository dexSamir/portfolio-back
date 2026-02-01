using System.Net;

namespace Portfolio.Application.Exceptions.Image;

public class UnsupportedFileTypeException(string message, string? errorCode = null, int code = 0)
    : BaseException(message, HttpStatusCode.BadRequest, errorCode, code);