using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Extensions.ClaimExtensions
{
    public static class HttpContextExtension
    {
        public static String GetUsersId(this HttpContext httpContext) => httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        public static String GetUsersFirstName(this HttpContext httpContext) => httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        public static String GetUsersLastName(this HttpContext httpContext) => httpContext.User.FindFirst(ClaimTypes.Surname)?.Value;
        public static String GetUsersEmail(this HttpContext httpContext) => httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        public static String[] GetUsersRoles(this HttpContext httpContext) => httpContext.User.FindAll(ClaimTypes.Role)?.Select(e => e.Value).ToArray();
    }
}
