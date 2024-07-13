using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Common.Exceptions;

namespace Scrumboard.Web.ExceptionHandlers;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    // Register known exception types and handlers.
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers = new()
    {
        { typeof(ValidationException), HandleValidationExceptionAsync },
        { typeof(NotFoundException), HandleNotFoundExceptionAsync },
        { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessExceptionAsync },
        { typeof(ForbiddenAccessException), HandleForbiddenAccessExceptionAsync },
    };
    
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (!_exceptionHandlers.TryGetValue(exceptionType, out var handler))
        {
            return false;
        }

        await handler.Invoke(httpContext, exception);
        
        return true;
    }

    private static async Task HandleValidationExceptionAsync(HttpContext httpContext, Exception ex)
    {
        var exception = (ValidationException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(exception.Errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        });
    }

    private static async Task HandleNotFoundExceptionAsync(HttpContext httpContext, Exception ex)
    {
        var exception = (NotFoundException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            //Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        });
    }

    private static async Task HandleUnauthorizedAccessExceptionAsync(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            Title = "Unauthorized"
        });
    }

    private static async Task HandleForbiddenAccessExceptionAsync(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            Title = "Forbidden"
        });
    }
}
