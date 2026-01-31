using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Exceptions;

namespace Portfolio.WebAPI.Middlewares;

public sealed class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger,
    IWebHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception exception)
    {
        var problem = new ProblemDetails
        {
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case BaseException baseEx:
                problem.Status = (int)baseEx.StatusCode;
                problem.Title = baseEx.Message;

                problem.Extensions["errorCode"] = baseEx.ErrorCode;
                problem.Extensions["code"] = baseEx.Code;
                break;

            case UnauthorizedAccessException:
                problem.Status = StatusCodes.Status401Unauthorized;
                problem.Title = "Unauthorized access";
                break;

            case FluentValidation.ValidationException validationEx:
                problem.Status = StatusCodes.Status400BadRequest;
                problem.Title = "Validation error";
                problem.Extensions["errors"] = validationEx.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );
                break;

            default:
                problem.Status = StatusCodes.Status500InternalServerError;
                problem.Title = "Internal Server Error";
                break;
        }

        if (env.IsDevelopment())
        {
            problem.Detail = exception.StackTrace;
            problem.Extensions["exceptionType"] = exception.GetType().Name;
        }

        logger.LogError(
            exception,
            "Exception caught | Status: {Status} | Path: {Path}",
            problem.Status,
            context.Request.Path
        );

        context.Response.StatusCode = problem.Status ?? 500;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problem)
        );
    }
}