using System.Text.Json;

namespace TransitPulse.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = 500;

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
    }
}