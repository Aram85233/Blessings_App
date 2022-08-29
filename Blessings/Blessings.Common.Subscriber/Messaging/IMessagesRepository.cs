using Blessings.Common.BackgroundWorker.Messaging;

namespace Blessings.Common.Subscriber.Messaging
{
    public interface IMessagesRepository
    {
        void Add(Message message);
        IReadOnlyCollection<Message> GetMessages();
    }
}