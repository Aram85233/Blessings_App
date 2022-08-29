using Blessings.Common.AspNet.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Blessings.Common.AspNet.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = (ex is HttpRequestException || ex is ValidationException) ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError;
            //if (ex is InvalidOperationException)
            //{
            //    code = HttpStatusCode.Unauthorized;
            //}

            var error = new ValidationMessageViewModel()
            {
                Message = $"exception message: {ex?.InnerException?.Message ?? ex?.Message ?? ""}",
                Status = (int)code
            };

            Serilog.Log.Error(ex?.ToString());
            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
