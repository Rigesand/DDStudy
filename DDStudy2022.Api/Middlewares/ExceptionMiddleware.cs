using DDStudy2022.Common.Exceptions;
using FluentValidation;

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
                catch (ValidationException exception)
                {
                    var errors = exception.Errors.Select(x => $"{x.ErrorMessage}");
                    var errorMessage = string.Join(Environment.NewLine, errors);

                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new {Message = errorMessage});
                }
                catch (UserException exception)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new {exception.Message});
                }
                catch (UserSessionException exception)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new {exception.Message});
                }
                catch (FileException exception)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new {exception.Message});
                }
                catch (PostException exception)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new {exception.Message});
                }
                catch (Exception)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new {Message = "Внутренняя ошибка сервера"});
                }
            });
        }
    }
}