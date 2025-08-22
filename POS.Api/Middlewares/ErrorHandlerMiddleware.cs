using Microsoft.AspNetCore.Mvc;
using WatchDog;

namespace POS.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            WatchLogger.Log(exception.ToString());

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)ExceptionMapping.GetStatusCode(exception);

            var problemDetails = new ProblemDetails
            {
                Status = response.StatusCode,
                Title = "An error occurred while processing your request",
                Detail = _env.IsDevelopment() ? exception.ToString() : exception.Message,
                Instance = context.Request.Path
            };

            return response.WriteAsJsonAsync(problemDetails);

        }

    }
}
