using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ChitChat.Api.Middleware
{
    public class HandleExceptionMiddleware
    {
        private readonly RequestDelegate _nextDelegate;

        public HandleExceptionMiddleware(RequestDelegate nextDelegate)
        {
            this._nextDelegate = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _nextDelegate(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            const string filePath = @"C:\ErrorLogs\Error.txt";

            var message = exception.Message;
            var ex = exception;
            await using (var writer = new StreamWriter(filePath, true))
            {
                await writer.WriteLineAsync("-----------------------------------------------------------------------------");
                await writer.WriteLineAsync("Date : " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
                await writer.WriteLineAsync();

                while (exception != null)
                {
                    await writer.WriteLineAsync(exception.GetType().FullName);
                    await writer.WriteLineAsync("Message : " + exception.Message);
                    await writer.WriteLineAsync("StackTrace : " + exception.StackTrace);

                    exception = exception.InnerException;
                }
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new {Message = message, code = HttpStatusCode.InternalServerError});
        }
    }
}
