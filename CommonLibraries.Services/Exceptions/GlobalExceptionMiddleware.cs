
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Libraries.Services.Exceptions
{
    public static class GlobalExceptionMiddleware
    {
        public static void UseGlobalExceptionMiddleware(this IApplicationBuilder app,Dictionary<string, int> errors)
        {
            app.UseMiddleware<CustomExceptionMiddleware>(errors);
        }
    }
}
