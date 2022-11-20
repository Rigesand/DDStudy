namespace DDStudy2022.Api.Middlewares.Extensions;

public static class StopDdosMiddlewareMiddlewareExtensions
{
    public static IApplicationBuilder UseAntiDdosCustom(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<StopDdosMiddleware>();
    }
}