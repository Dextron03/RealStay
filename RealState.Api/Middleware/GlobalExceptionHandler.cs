using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace RealStay.API.Middleware
{
    public class GlobalExceptionHandler : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            var (statusCode, title) = exception switch
            {
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "No autorizado"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
                _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
            };

            context.Response.StatusCode = statusCode;

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            var json = JsonSerializer.Serialize(problem);
            return context.Response.WriteAsync(json);
        }
    }
}