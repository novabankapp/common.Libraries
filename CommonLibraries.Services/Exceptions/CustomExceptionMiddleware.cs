
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.Services.Exceptions
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        private readonly Dictionary<string, int> _exceptionHandlersStatusCodes;
        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger, Dictionary<string, int> errors)
        {
            _logger = logger;
            _next = next;
            _exceptionHandlersStatusCodes = new Dictionary<string, int>()
            {
                {"Common.Libraries.Services.Exceptions.EntityNotFoundException",404 },
                {"CommonLibraries.Services.Exceptions.ValidationException",401 }
            };
            foreach (var error in errors)
            {
                if(!_exceptionHandlersStatusCodes.ContainsKey(error.Key))
                   _exceptionHandlersStatusCodes.Add(error.Key,error.Value);
            }
            
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occur: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var type = exception.GetType().FullName;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            if (_exceptionHandlersStatusCodes.ContainsKey(type))
            {
                int code = _exceptionHandlersStatusCodes[type];
                context.Response.StatusCode = code;
            }

           // Utils.Helpers.WriteLog("", exception);
        
            return context.Response.WriteAsync(new CustomError()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
            }.ToString());
        }
    }
}
