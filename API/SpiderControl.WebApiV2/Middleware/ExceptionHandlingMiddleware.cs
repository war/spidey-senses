using SpiderControl.Core.Exceptions;
using System.Net;

namespace SpiderControl.WebApiV2.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new ErrorResponse();

        var (status, error) = exception switch
        {
            InputParseException ex => (HttpStatusCode.BadRequest,
                new ErrorResponse("Invalid Input", ex.Message)),
            ModelValidationException ex => (HttpStatusCode.BadRequest,
                new ErrorResponse("Validation Error", ex.Message)),
            InvalidOperationException ex => (HttpStatusCode.BadRequest,
                new ErrorResponse("Invalid Operation", ex.Message)),
            _ => (HttpStatusCode.InternalServerError,
                new ErrorResponse("Internal Server Error", "An unexpected error occurred"))
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        await context.Response.WriteAsJsonAsync(error);
    }
}
