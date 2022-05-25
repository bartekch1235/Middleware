using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UAParser;
namespace Middleware.Apps
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ZmianaStrony
    {
        private readonly RequestDelegate _next;

        public ZmianaStrony(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var uaString = httpContext.Request.Headers["User-Agent"];
            var parser = Parser.GetDefault();

            ClientInfo client = parser.Parse(uaString);
            string browser = client.UA.Family;

            if (browser == "Edge" || browser == "IE" )
            {
                httpContext.Response.Redirect($"https://www.wp.pl");
            }
            await _next(httpContext);

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseZmianastrony(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ZmianaStrony>();
        }
    }
}
