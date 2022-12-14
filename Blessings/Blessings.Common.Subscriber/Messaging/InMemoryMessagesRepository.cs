using Blessings.Common.BackgroundWorker.Messaging;

namespace Blessings.Common.Subscriber.Messaging
{
    public class InMemoryMessagesRepository : IMessagesRepository
    {
        private readonly Queue<Message> _messages;

        public InMemoryMessagesRepository()
        {
            _messages = new Queue<Message>();
        }

        public void Add(Message message)
        {
            _messages.Enqueue(message ?? throw new ArgumentNullException(nameof(message)));
        }

        public IReadOnlyCollection<Message> GetMessages()
        {
            return _messages.ToArray();
        }
    }
}