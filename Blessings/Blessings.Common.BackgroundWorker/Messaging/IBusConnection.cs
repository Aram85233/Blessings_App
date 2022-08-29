using RabbitMQ.Client;

namespace Blessings.Common.BackgroundWorker.Messaging
{
    public interface IBusConnection
    {
        bool IsConnected { get; }

        IModel CreateChannel();
    }
}
