using Blessings.Data;
using Blessings.Services.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blessings.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseOptions = configuration.GetSection(DatabaseOptions.Section).Get<DatabaseOptions>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = databaseOptions.ConnectionString;
                options.UseSqlServer(connectionString, builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });

            return services;
        }
    }
}
