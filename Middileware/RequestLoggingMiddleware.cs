using ContactManagement.Application.Common.Interfaces;
using System.Diagnostics;
using System.Text;

namespace phonebookApi.Middileware
{

    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly DiagnosticSource _diagnosticSource;

        public RequestLoggingMiddleware( RequestDelegate next,ILogger<RequestLoggingMiddleware> logger,ICurrentUserService currentUserService, DiagnosticSource diagnosticSource)
        {
            _next = next;
            _logger = logger;
            _currentUserService = currentUserService;
            _diagnosticSource = diagnosticSource;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await LogRequest(context);
                await _next(context);
            }
            finally
            {
                LogResponse(context);
            }
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            var requestBody = string.Empty;
            if (context.Request.ContentLength > 0)
            {
                using var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true);

                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var userId = _currentUserService.UserId;

            _logger.LogInformation(
                "HTTP Request Information:\n" +
                "Schema:{Schema}\n" +
                "Host: {Host}\n" +
                "Path: {Path}\n" +
                "QueryString: {QueryString}\n" +
                "Request Body: {RequestBody}\n" +
                "User ID: {UserId}",
                context.Request.Scheme,
                context.Request.Host,
                context.Request.Path,
                context.Request.QueryString,
                requestBody,
                userId);

            if (_diagnosticSource.IsEnabled("PhonebookApi.RequestLogging"))
            {
                _diagnosticSource.Write(
                    "PhonebookApi.RequestStart",
                    new
                    {
                        httpContext = context,
                        timestamp = DateTime.UtcNow
                    });
            }
        }

        private void LogResponse(HttpContext context)
        {
            _logger.LogInformation(
                "HTTP Response Information:\n" +
                "Status Code: {StatusCode}\n" +
                "Content Type: {ContentType}",
                context.Response.StatusCode,
                context.Response.ContentType);

            if (_diagnosticSource.IsEnabled("PhonebookApi.RequestLogging"))
            {
                _diagnosticSource.Write(
                    "PhonebookApi.RequestEnd",
                    new
                    {
                        httpContext = context,
                        timestamp = DateTime.UtcNow
                    });
            }
        }
    }

    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging( this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
