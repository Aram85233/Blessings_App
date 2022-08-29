using Blessings.Common.BackgroundWorker.Messaging;

namespace Blessings.Publisher
{
    public interface IRabbitPublisher : IDisposable
    {
        public void Publish(Message message);
    }
}
