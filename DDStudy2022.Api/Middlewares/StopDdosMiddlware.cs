using DDStudy2022.Api.Services;
using DDStudy2022.Common.Exceptions;

namespace DDStudy2022.Api.Middlewares;

public class StopDdosMiddleware
{
    private readonly RequestDelegate _next;

    public StopDdosMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, DdosGuard guard)
    {
        var headerAuth = context.Request.Headers.Authorization;

        try
        {
            guard.CheckRequest(headerAuth);
            await _next(context);
        }
        catch (TooManyRequestException)
        {
            context.Response.StatusCode = 429;
            await context.Response.WriteAsJsonAsync("too many Requests, allowed 10 request per second");
        }
    }
}