using System.Net;
using Kosa.ECommerce.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Kosa.ECommerce.Server.Middleware;

public sealed class ExceptionHandlingMiddleware
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

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (
            context.RequestAborted.IsCancellationRequested)
        {
            _logger.LogDebug(
                "Request cancelled by client: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            // Don't try to write a response because the client
            // has already cancelled the request.
        }
        catch (ValidationException ex)
        {
            await WriteProblemAsync(
                context,
                HttpStatusCode.BadRequest,
                "Validation Error",
                ex.Message);
        }
        catch (NotFoundException ex)
        {
            await WriteProblemAsync(
                context,
                HttpStatusCode.NotFound,
                "Not Found",
                ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            await WriteProblemAsync(
                context,
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                ex.Message);
        }
        catch (ConflictException ex)
        {
            await WriteProblemAsync(
                context,
                HttpStatusCode.Conflict,
                "Conflict",
                ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception while processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await WriteProblemAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred. Please try again later.");
        }
    }

    private static async Task WriteProblemAsync(
        HttpContext context,
        HttpStatusCode status,
        string title,
        string detail)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = (int)status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}