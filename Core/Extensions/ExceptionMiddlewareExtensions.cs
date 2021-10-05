using Microsoft.AspNetCore.Builder;

namespace Core.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app) // extend edilir.
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
