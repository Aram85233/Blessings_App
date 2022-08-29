using Blessings.Common.BackgroundWorker.Messaging;
using Blessings.Common.Subscriber.Messaging;
using Blessings.Common.Subscriber.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Blessings.Common.Subscriber
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSubscriber(this IServiceCollection services, Action<RabbitMQOptions> confOptions)
        {
            var option = new RabbitMQOptions();
            confOptions.Invoke(option);
            return services.AddSubscriber(option);
        }
        public static IServiceCollection AddSubscriber(this IServiceCollection services, RabbitMQOptions option)
        {
            services.AddSingleton<IMessagesRepository, InMemoryMessagesRepository>();
      
            var exchangeName = option.Exchange;
            var queueName = option.Queue;
            var deadLetterExchange = option.DeadLetterExchange;
            var deadLetterQueue = option.DeadLetterQueue;
            var subscriberOptions = new RabbitSubscriberOptions(exchangeName, queueName, deadLetterExchange, deadLetterQueue);
            services.AddSingleton(subscriberOptions);

            var connectionFactory = new ConnectionFactory()
            {
                HostName = option.HostName,
                UserName = option.UserName,
                Password = option.Password,
                DispatchConsumersAsync = true
            };
            services.AddSingleton<IConnectionFactory>(connectionFactory);

            services.AddSingleton<IBusConnection, RabbitPersistentConnection>();
            services.AddSingleton<ISubscriber, RabbitSubscriber>();

            var channel = System.Threading.Channels.Channel.CreateBounded<Message>(100);
            services.AddSingleton(channel);

            services.AddSingleton<IProducer>(ctx =>
            {
                var channel = ctx.GetRequiredService<System.Threading.Channels.Channel<Message>>();
                var logger = ctx.GetRequiredService<ILogger<Producer>>();
                return new Producer(channel.Writer, logger);
            });

            services.AddSingleton<IEnumerable<IConsumer>>(ctx =>
            {
                var channel = ctx.GetRequiredService<System.Threading.Channels.Channel<Message>>();
                var logger = ctx.GetRequiredService<ILogger<Consumer>>();
                var repo = ctx.GetRequiredService<IMessagesRepository>();

                var consumers = Enumerable.Range(1, 10)
                                          .Select(i => new Consumer(channel.Reader, logger, i, repo))
                                          .ToArray();
                return consumers;
            });

            services.AddHostedService<BackgroundSubscriberWorker>();

            return services;
        }
    }
}
