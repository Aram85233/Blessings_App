using Blessings.Common.BackgroundWorker.Messaging;
using Blessings.Common.Publisher.Options;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Blessings.Common.Publisher
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPublisher(this IServiceCollection services, Action<RabbitMQPublisherOptions> confOptions)
        {
            var option = new RabbitMQPublisherOptions();
            confOptions.Invoke(option);
            return services.AddPublisher(option);
        }

        public static IServiceCollection AddPublisher(this IServiceCollection services, RabbitMQPublisherOptions confOptions)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = confOptions.HostName,
                UserName = confOptions.UserName,
                Password = confOptions.Password
            };

            services.AddSingleton<IConnectionFactory>(connectionFactory);
            services.AddScoped<IRabbitPublisher, RabbitPublisher>();
            services.AddSingleton<IBusConnection, RabbitPersistentConnection>();

            return services;
        }
    }
}
