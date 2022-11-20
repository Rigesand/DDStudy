using DDStudy2022.Api.Interfaces;
using DDStudy2022.Common.Consts;
using DDStudy2022.Common.Exstensions;

namespace DDStudy2022.Api.Middlewares;

public class TokenValidatorMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidatorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        var isOk = true;
        var sessionId = context.User.GetClaimValue<Guid>(ClaimNames.SessionId);
        if (sessionId != default)
        {
            var session = await authService.GetSessionById(sessionId);
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