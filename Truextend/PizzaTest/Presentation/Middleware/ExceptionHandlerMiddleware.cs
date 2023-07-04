using Newtonsoft.Json;
using System.Net;
using Truextend.PizzaTest.Data.Exceptions;
using Truextend.PizzaTest.Logic.Exceptions;

namespace Truextend.PizzaTest.Presentation.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private const string _jsonContentType = "application/json";

        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
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

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var ErrorResponse = new MiddlewareResponse<string>(null);

            if (ex is UnauthorizedAccessException || ex is ArgumentNullException)
            {
                ErrorResponse.status = (int)HttpStatusCode.Unauthorized;
                ErrorResponse.error.message = "Unauthorized";
            }
            else if (ex is LogicException)
            {
                ErrorResponse.status = (int)HttpStatusCode.OK;
                ErrorResponse.error.message = "Logic Exception" + Environment.NewLine + "Message: " + ex.Message + Environment.NewLine;
            }
            else if (ex is DatabaseException)
            {
                ErrorResponse.status = (int)HttpStatusCode.OK;
                ErrorResponse.error.message = $"Data Error{Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
            }
            else if (ex is BadRequestException)
            {
                ErrorResponse.status = (int)HttpStatusCode.BadRequest;
                ErrorResponse.error.message = $"Data Error{Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
                if (((BadRequestException)ex).Details != null)
                {
                    ErrorResponse.error.details = ((BadRequestException)ex).Details;
                }
            }
            else if (ex is NotFoundException)
            {
                ErrorResponse.status = (int)HttpStatusCode.NotFound;
                ErrorResponse.error.message = $"Data Error{Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
            }
            else if (ex is AlreadyExistException)
            {
                ErrorResponse.status = (int)HttpStatusCode.BadRequest;
                ErrorResponse.error.message = $"Data Error{Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
            }
            else
            {
                ErrorResponse.status = (int)HttpStatusCode.InternalServerError;
                ErrorResponse.error.message = "Internal Server Error: " + ex.Message;
            }

            context.Response.ContentType = _jsonContentType;
            context.Response.StatusCode = ErrorResponse.status;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(ErrorResponse));
        }
    }
}
