using System.Net;
using System.Text.Json;

namespace CRN.API.Middleware
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
                _logger.LogError(
                    ex,
                    "An unhandled exception occurred. TraceId: {TraceId}",
                    context.TraceIdentifier);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                KeyNotFoundException =>
                    HttpStatusCode.NotFound,

                InvalidOperationException =>
                    HttpStatusCode.Conflict,

                ArgumentException =>
                    HttpStatusCode.BadRequest,

                _ =>
                    HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var message = exception switch
            {
                KeyNotFoundException =>
                    exception.Message,

                InvalidOperationException =>
                    exception.Message,

                ArgumentException =>
                    exception.Message,

                _ =>
                    "An unexpected error occurred."
            };

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = message,
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}