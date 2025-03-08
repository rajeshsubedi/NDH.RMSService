using DomainLayer.Exceptions;
using DomainLayer.Wrappers.GlobalResponse;
using System.Net;
using System.Text.Json;

namespace RMSServiceAPI.CustomMiddlewareExceptions
{
    public class CustomExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            var response = httpContext.Response;
            var errorResponse = new BaseResponse<string?>
            {
                _success = false,
                _data = null
            };

            switch (exception)
            {
                case UserUnauthenticatedException ex:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse._statusCode = HttpStatusCode.Unauthorized;
                    errorResponse._message = ex.Message;
                    break;

                case CustomInvalidOperationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse._statusCode = HttpStatusCode.BadRequest;
                    errorResponse._message = ex.Message;
                    break;

                case DuplicateRecordException ex:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse._statusCode = HttpStatusCode.Conflict;
                    errorResponse._message = ex.Message;
                    break;

                case NotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse._statusCode = HttpStatusCode.NotFound;
                    errorResponse._message = ex.Message;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse._statusCode = HttpStatusCode.InternalServerError;
                    errorResponse._message = "Internal Server Error!";
                    break;
            }

            string result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await httpContext.Response.WriteAsync(result);
        }
    }

}
