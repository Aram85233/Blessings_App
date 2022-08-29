using AutoMapper;
using Blessings.Common.AspNet.Extensions;
using Blessings.Common.Publisher;
using Blessings.Common.Publisher.Options;
using Blessings.Common.Subscriber;
using Blessings.Common.Subscriber.Options;
using Blessings.Options;
using Blessings.Services;
using Blessings.Services.Contracts;
using Blessings.Services.Impl;
using Blessings.Services.Options;
using Hangfire;
using System.Text;

namespace Blessings
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlessingsLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBlessingsOptions(configuration);

            services.AddAllDbContexts(configuration);

            services.AddBlessingsSwagger();

            services.AddBlessingsServices(configuration);

            var section = configuration.GetSection(JwtTokenOptions.Section);
            var secret = Encoding.ASCII.GetBytes(section[nameof(JwtTokenOptions.Secret)]);

            services.AddAuthenticationLayer(secret);

            services.AddHangfire(configuration);

            return services;
        }

        public static IServiceCollection AddBlessingsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            var publisherOptions = configuration.GetSection(RabbitMQPublisherOptions.Section);
            var subscriberOptions = configuration.GetSection(RabbitMQOptions.Section);

            services.AddPublisher(x =>
            {
                x.UserName = publisherOptions["UserName"];
                x.Password = publisherOptions["Password"];
                x.HostName = publisherOptions["HostName"];
                x.Exchange = publisherOptions["Exchange"];
            });
            services.AddSubscriber(x =>
            {
                x.UserName = subscriberOptions["UserName"];
                x.Password = subscriberOptions["Password"];
                x.HostName = subscriberOptions["HostName"];
                x.Exchange = subscriberOptions["Exchange"];
                x.Queue = subscriberOptions["Queue"];
                x.DeadLetterExchange = subscriberOptions["DeadLetterExchange"];
                x.DeadLetterQueue = subscriberOptions["DeadLetterQueue"];
            });
            return services;
        }

        public static IServiceCollection AddBlessingsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtTokenOptions>(o =>
            {
                var section = configuration.GetSection(JwtTokenOptions.Section);
                var key = Encoding.ASCII.GetBytes(section[nameof(JwtTokenOptions.Secret)]);
                o.Secret = key;
                o.AccessTokenDurationInMinutes = Convert.ToInt32(section[nameof(JwtTokenOptions.AccessTokenDurationInMinutes)]);
                o.AccessTokenDurationInMinutesRememberMe = Convert.ToInt32(section[nameof(JwtTokenOptions.AccessTokenDurationInMinutesRememberMe)]);
            });

            services.Configure<RabbitMQOptions>(configuration.GetSection(RabbitMQOptions.Section));
            services.Configure<RabbitMQPublisherOptions>(configuration.GetSection(RabbitMQPublisherOptions.Section));

            return services;
        }

        public static IServiceCollection AddBlessingsSwagger(this IServiceCollection services)
        {
            services.AddSwaggerLayer();

            return services;
        }

        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseOptions = configuration.GetSection(DatabaseOptions.Section).Get<DatabaseOptions>();

            services.AddHangfire(config =>
               config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseDefaultTypeSerializer()
               .UseSqlServerStorage(databaseOptions.ConnectionString));

            services.AddHangfireServer();

            services.AddSingleton<IOrderService, OrderService>();

            return services;
        }

        public static IServiceProvider CollectOrders(this IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var orderOption = configuration.GetSection(OrderOptions.Section).Get<OrderOptions>();

            //var backgroundJobClient = serviceProvider.GetRequiredService<IBackgroundJobClient>();
            var recurringJobManager = serviceProvider.GetRequiredService<IRecurringJobManager>();

            //backgroundJobClient.Enqueue(() => Console.WriteLine("Hello Hanfire job!"));
            recurringJobManager.AddOrUpdate(
                "Orders",
                () => serviceProvider.GetService<IOrderService>().GetOrders(),
                orderOption.CronExpression
                );

            return serviceProvider;
        }
    }
}
