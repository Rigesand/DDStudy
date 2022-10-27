namespace DDStudy2022.Api.Middlewares
{
    public static class ExceptionMiddleware
    {
        public static void UseValidationException(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                try
                {
                    await next(context);
                }
                catch (FluentValidation.ValidationException exception)
                {
                    var errors = exception.Errors.Select(x => $"{x.ErrorMessage}");
                    var errorMessage = string.Join(Environment.NewLine, errors);
                    
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new {Message = errorMessage});
                }
                catch (Exception)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { Message = "Внутренняя ошибка сервера" });
                }
            });
        }
    }
}