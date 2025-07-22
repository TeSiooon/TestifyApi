using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Testify.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        //catch (NotFoundException notFound)
        //{
        //    context.Response.StatusCode = 404;
        //    await context.Response.WriteAsync(notFound.Message);

        //    logger.LogWarning(notFound.Message);
        //}
        //catch (ForbidException)
        //{
        //    context.Response.StatusCode = 403;
        //    await context.Response.WriteAsync("Access forbidden");
        //}
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case KeyNotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = ex.Message
                    }));
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = "Unauthorized"
                    }));
                    break;

                case ValidationException validationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = "Validation error",
                        details = validationException.Errors.Select(e => e.ErrorMessage)
                    }));
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = "Something went wrong"
                    }));
                    break;
            }
        }
    }
}