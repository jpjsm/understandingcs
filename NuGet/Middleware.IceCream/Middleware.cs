using Microsoft.AspNetCore.Builder;
using Middleware.IceCream.Metrics;

namespace Middleware.IceCream
{
    public static class Middleware
    {
        public static IApplicationBuilder UseAspNetMetrics(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AspNetMetrics>();
        }
    }
}
