using Blessings.Common.BackgroundWorker.Messaging;
using Blessings.Data.Entities;

namespace Blessings.Services.Contracts
{
    public interface IOrderService
    {
        void PublishOrder(Order order);
        IReadOnlyCollection<Message> CollectOrders();
        Task AddOrdersAsync(IEnumerable<Order> orders);
    }
}
