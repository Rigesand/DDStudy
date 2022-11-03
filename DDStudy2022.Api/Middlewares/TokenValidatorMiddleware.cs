using DDStudy2022.Api.Interfaces;

namespace DDStudy2022.Api.Middlewares;

public class TokenValidatorMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidatorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserService userService)
    {
        var isOk = true;
        var sessionIdString = context.User.Claims.FirstOrDefault(x => x.Type == "sessionId")?.Value;
        if (Guid.TryParse(sessionIdString, out var sessionId))
        {
            var session = await userService.GetSessionById(sessionId);
            if (!session.IsActive)
            {
                isOk = false;
                context.Response.Clear();
                context.Response.StatusCode = 401;
            }
        }

        if (isOk)
        {
            await _next(context);
        }
    }
}