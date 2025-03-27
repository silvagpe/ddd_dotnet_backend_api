using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace DeveloperStore.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (ex is ResourceNotFoundException || ex is AuthenticationException || ex is ValidationException)
            {
                _logger.LogWarning(ex, "An error occurred while processing the request.");
            }
            else
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
            }            
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            ResourceNotFoundException ex => new
            {
                type = "ResourceNotFound",
                error = ex.Message,
                detail = ex.Detail
            },
            AuthenticationException ex => new
            {
                type = "AuthenticationError",
                error = ex.Message,
                detail = ex.Detail
            },
            ValidationException ex => new
            {
                type = "ValidationError",
                error = ex.Message,
                detail = ex.Detail
            },
            _ => new
            {
                type = "InternalServerError",
                error = "An unexpected error occurred",
                detail = exception.Message
            }
        };

        response.StatusCode = exception switch
        {
            ResourceNotFoundException => (int)HttpStatusCode.NotFound,
            AuthenticationException => (int)HttpStatusCode.Unauthorized,
            ValidationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        return response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}

// Custom exception classes
public class ResourceNotFoundException : Exception
{
    public string Detail { get; }

    public ResourceNotFoundException(string message, string detail) : base(message)
    {
        Detail = detail;
    }
}

public class AuthenticationException : Exception
{
    public string Detail { get; }

    public AuthenticationException(string message, string detail) : base(message)
    {
        Detail = detail;
    }
}

public class ValidationException : Exception
{
    public string Detail { get; }

    public ValidationException(string message, string detail) : base(message)
    {
        Detail = detail;
    }
}

