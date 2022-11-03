namespace DDStudy2022.Api.Middlewares.Extensions;

public static class TokenValidatorMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenValidator(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenValidatorMiddleware>();
    }
}