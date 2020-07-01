using Microsoft.AspNetCore.Http;
using PlayersWallet.OpenApi.Errors;
using PlayersWallet.Persistence.Exceptions;
using Serilog;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using System;

namespace PlayersWallet.OpenApi.Middleware
{
    // inspired by: https://code-maze.com/global-error-handling-aspnetcore/
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            Log.Error($"path: {context.Request.Path}, {ex.Message}");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)GetStatusCode(ex);
            var errorMsg = ex.Message;
            if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
            {
                errorMsg = ex.InnerException.Message;
            }
            await context.Response.WriteAsync(new ApiError(context.Response.StatusCode, errorMsg).ToString()).ConfigureAwait(false);
        }

        private static HttpStatusCode GetStatusCode(Exception ex)
        {
            switch (ex)
            {
                case RepositoryNotFoundException _:
                    return HttpStatusCode.NotFound;
                case FormatException _:
                    return HttpStatusCode.BadRequest;
                case AuthenticationException _:
                    return HttpStatusCode.Forbidden;
                case NotImplementedException _:
                    return HttpStatusCode.NotImplemented;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }
}
