using System.Net;
using System.Text.Json;
using TaskApi.Models;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Microsoft.Graph.ServiceException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized || ex.ResponseStatusCode == (int)HttpStatusCode.Forbidden)
        {
            _logger.LogWarning("Graph permission error: {Message}", ex.Message);

            var errorResponse = new ErrorResponse
            {
                StatusCode = ex.ResponseStatusCode > 0 ? ex.ResponseStatusCode : 500,
                Message = "Permission denied. Please check application permissions."
            };

            context.Response.StatusCode = errorResponse.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            var errorResponse = new ErrorResponse
            {
                StatusCode = 500,
                Message = "An unexpected error occurred."
            };

            context.Response.StatusCode = errorResponse.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
