
using System.Net;

namespace CodePulse.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                //Logging Exception
                logger.LogError(ex,$"{errorId}: {ex.Message}");

                //Returning a custom error response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    id = errorId,
                    message = "Something went wrong pls share the logs to developer"
                };

               await httpContext.Response.WriteAsJsonAsync(error);
                
            }
        }
    }
}
