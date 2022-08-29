using Blessings.Common.BackgroundWorker.Messaging;

namespace Blessings.Common.Publisher
{
    public interface IRabbitPublisher : IDisposable
    {
        public void Publish(Message message);
    }
}
