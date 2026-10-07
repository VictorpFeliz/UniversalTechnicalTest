using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Text.Json;
using UniversalTechnicalTest.Api.Exceptions;

namespace UniversalTechnicalTest.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware( RequestDelegate nexr, ILogger<ExceptionMiddleware> logger)
        {
            _next = nexr;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException ex)
            {
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Message);
                
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Ocurrio un error interno en el servidor.");
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                status = context.Response.StatusCode,
                message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
