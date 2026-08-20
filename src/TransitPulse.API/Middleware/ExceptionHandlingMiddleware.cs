using System.Text.Json;
using TransitPulse.Application.Exceptions;
using TransitPulse.API.Contracts;

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
        catch (FluentValidation.ValidationException exception)
        {
            context.Response.StatusCode = 400;

            context.Response.ContentType =
                "application/json";

            var response = new ErrorResponse(
                        "validation_error",
                        exception.Errors
                            .Select(error => error.ErrorMessage)
                            .ToList());

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
        catch (NotFoundException exception)
        {
            context.Response.StatusCode = 404;

            context.Response.ContentType =
                "application/json";

            var response = new ErrorResponse(
            "not_found",
            new List<string>
            {
                exception.Message
            });

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }

        catch (ConflictException exception)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;

            context.Response.ContentType = "application/json";

            var response = new ErrorResponse(
                "conflict",
                new List<string>
                {
            exception.Message
                });

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }

        catch (BadRequestException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            context.Response.ContentType = "application/json";

            var response = new ErrorResponse(
                "bad_request",
                exception.Errors.ToList());

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }

        catch (UnauthorizedException exception)
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            context.Response.ContentType =
                "application/json";

            var response = new ErrorResponse(
                "unauthorized",
                new List<string>
                {
            exception.Message
                });

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

            var response = new ErrorResponse(
                "internal_server_error",
                new List<string>
                {
                    "An unexpected error occurred."
                });

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}