using System.Text.Json; 
namespace SmartShip.ShipmentService.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        // _next represnts next componenet in http pipeline

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            //ASP.NET Core gives our middleware the next pipeline component.
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // context is info about current http request
                //Continue processing this request and go to the next middleware.
            }
            catch (ArgumentException ex) // we have used  it 
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = ex.Message // we have already written the message in validation
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }

            catch (Exception) // every other exception
            {
                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "An unexpected error occured."
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }
}
