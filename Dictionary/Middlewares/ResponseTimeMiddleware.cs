using System.Diagnostics;
using System.Threading.Tasks;
using Dictionary.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Dictionary.Middlewares
{
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;
        public ResponseTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();

            context.Response.OnStarting(state =>
            {
                watch.Stop();
                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Add("X-Response-Time-Milliseconds", new[] { watch.ElapsedMilliseconds.ToString() });
                return Task.FromResult(0);
            }, context);

            await _next(context);
        }
    }

}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseTime(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseTimeMiddleware>();
        }
    }
}