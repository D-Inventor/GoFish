using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace GoFish.Web.Middleware
{
    public class UserIdMiddleWare
    {
        private readonly RequestDelegate _next;

        public UserIdMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Guid userId;
            if (httpContext.Request.Cookies.TryGetValue("UserSession", out string UserSession))
            {
                userId = Guid.Parse(UserSession);
            }
            else
            {
                userId = Guid.NewGuid();
            }

            httpContext.Items["GameUserId"] = userId;

            httpContext.Response.Cookies.Append("UserSession", userId.ToString(), new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(5)
            });

            await _next(httpContext);
        }
    }

    public static class UserIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseGameUserContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserIdMiddleWare>();
        }
    }
}
