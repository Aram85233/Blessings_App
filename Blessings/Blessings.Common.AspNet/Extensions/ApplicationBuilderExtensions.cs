using Blessings.Common.AspNet.Middlewares;
using Microsoft.AspNetCore.Builder;
namespace Blessings.Common.AspNet.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddAuthenticationConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<AuthenticationMiddleware>();

            return app;
        }
    }
}
