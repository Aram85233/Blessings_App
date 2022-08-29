namespace Blessings.Common.Subscriber.Messaging
{
    public interface IConsumer
    {
        Task BeginConsumeAsync(CancellationToken cancellationToken = default);
    }
}
