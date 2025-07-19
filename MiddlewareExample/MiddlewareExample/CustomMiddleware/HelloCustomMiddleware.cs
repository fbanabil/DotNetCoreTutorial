using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MiddlewareExample.CustomMiddleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HelloCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public HelloCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //await httpContext.Response.ContentType = "text/plain";
            if(httpContext.Request.Query.ContainsKey("name"))
            {
                var name = httpContext.Request.Query["name"].ToString();
                await httpContext.Response.WriteAsync($"Hello, {name}!\n");
            }
            else
            {
                await httpContext.Response.WriteAsync("Hello, World!\n");
            }


            await httpContext.Response.WriteAsync("Hello, Custom Middleware!\n");
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HelloCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseHelloCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HelloCustomMiddleware>();
        }
    }
}
