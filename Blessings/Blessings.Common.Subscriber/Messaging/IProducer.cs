using Blessings.Common.BackgroundWorker.Messaging;

namespace Blessings.Common.Subscriber.Messaging
{
    public interface IProducer
    {
        Task PublishAsync(Message message, CancellationToken cancellationToken = default);
    }
}
