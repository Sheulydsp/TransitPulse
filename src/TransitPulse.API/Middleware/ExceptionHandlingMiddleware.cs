using System.Text.Json;
using TransitPulse.Application.Exceptions;

namespace TransitPulse.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = 400;

            context.Response.ContentType =
                "application/json";

            var response =
                new
                {
                    Error = exception.Message
                };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
        catch (NotFoundException exception)
        {
            context.Response.StatusCode = 404;

            context.Response.ContentType =
                "application/json";

            var response =
                new
                {
                    Error = exception.Message
                };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Unhandled exception occurred.");

            context.Response.StatusCode = 500;

            context.Response.ContentType =
                "application/json";

            var response =
                new
                {
                    Error = "An unexpected error occurred."
                };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}