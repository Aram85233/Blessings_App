using Blessings.Common.BackgroundWorker.Messaging;
using Blessings.Common.Publisher.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Blessings.Common.Publisher
{
    public class RabbitPublisher : IRabbitPublisher
    {
        private readonly IBusConnection _connection;
        private IModel _channel;
        private readonly IBasicProperties _properties;
        private readonly RabbitMQPublisherOptions _options;

        public RabbitPublisher(IBusConnection connection, IOptions<RabbitMQPublisherOptions> options)
        {
            _options = options.Value;
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _channel = _connection.CreateChannel();
            _channel.ExchangeDeclare(exchange: _options.Exchange, type: ExchangeType.Fanout);
            _properties = _channel.CreateBasicProperties();
        }

        public void Publish(Message message)
        {
            var jsonData = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(jsonData);

            _channel.BasicPublish(
                exchange: _options.Exchange,
                routingKey: string.Empty,
                mandatory: true,
                basicProperties: _properties,
                body: body);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _channel = null;
        }
    }
}
