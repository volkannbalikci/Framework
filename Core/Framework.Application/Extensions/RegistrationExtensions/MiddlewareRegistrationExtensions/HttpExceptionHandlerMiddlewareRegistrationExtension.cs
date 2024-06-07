using Framework.Application.CrossCuttingConcerns.ExceptionHandling;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Extensions.Registrations.MiddlewareRegistrationExtensions
{
    public static class HttpExceptionHandlerMiddlewareRegistrationExtension
    {
        public static void UseHttpExceptionHandlerMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<HttpExceptionHandler>();
        }
    }
}
