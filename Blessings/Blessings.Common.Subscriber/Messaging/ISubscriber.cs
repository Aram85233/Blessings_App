using RabbitMQ.Client.Events;

namespace Blessings.Common.Subscriber.Messaging
{
    public interface ISubscriber
    {
        void Start();
        event AsyncEventHandler<RabbitSubscriberEventArgs> OnMessage;
    }
}
