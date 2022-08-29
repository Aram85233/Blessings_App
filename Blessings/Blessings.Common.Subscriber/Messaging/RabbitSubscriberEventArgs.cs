using Blessings.Common.BackgroundWorker.Messaging;

namespace Blessings.Common.Subscriber.Messaging
{
    public class RabbitSubscriberEventArgs : EventArgs{
        public RabbitSubscriberEventArgs(Message message){
            this.Message = message;
        }

        public Message Message{get;}
    }
}
