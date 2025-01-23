using ContactManagement.Infrastructure.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace phonebookApi.Middileware
{
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
            var response = context.Response;
            response.ContentType = "application/json";

            // Map exception types to status codes
            response.StatusCode = exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            var errorResponse = new
            {
                message = exception.Message,
                statusCode = response.StatusCode,
                errorType = exception.GetType().Name
            };

            try
            {
                var result = JsonSerializer.Serialize(errorResponse);
                await response.WriteAsync(result);
            }
            catch (Exception serializationException)
            {
                // Handle potential serialization issues
                var fallbackError = new { message = "An error occurred while processing the error response.", statusCode = 500 };
                var fallbackResult = JsonSerializer.Serialize(fallbackError);
                await response.WriteAsync(fallbackResult);

            }
        }

    }

}
