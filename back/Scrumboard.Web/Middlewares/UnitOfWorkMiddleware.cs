using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Web.Middlewares;

public sealed class UnitOfWorkMiddleware(
    IUnitOfWork unitOfWork) : IMiddleware
{
    private static readonly string[] ReadHttpMethods =
    [
        HttpMethods.Get,
        HttpMethods.Head,
        HttpMethods.Options
    ];

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (IsReadHttpMethod(context.Request.Method))
        {
            await next.Invoke(context);
        }
        else
        {
            await unitOfWork.CommitAsync(() => next.Invoke(context), context.RequestAborted);
        }
    }

    private static bool IsReadHttpMethod(string method)
        => ReadHttpMethods.Contains(method, StringComparer.OrdinalIgnoreCase);
}
