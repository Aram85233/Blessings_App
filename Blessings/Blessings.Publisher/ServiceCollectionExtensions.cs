using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Blessings.Publisher
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMQPublisher(this IServiceCollection services, Action<IConnectionFactory> configureConnection)
        {
            var connection = new ConnectionFactory();
            configureConnection.Invoke(connection);

            return services.AddRabbitMQPublisher(connection);
        }

        public static IServiceCollection AddRabbitMQPublisher(this IServiceCollection services, IConnectionFactory connection) 
        {
            services.AddScoped<IConnectionFactory, ConnectionFactory>();

            services.AddScoped<IRabbitPublisher, RabbitPublisher>();

            return services;
        }
            
    }
}
