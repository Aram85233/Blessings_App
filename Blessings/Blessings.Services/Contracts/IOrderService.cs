using Blessings.Common.BackgroundWorker.Messaging;
using Blessings.Data.Entities;

namespace Blessings.Services.Contracts
{
    public interface IOrderService
    {
        void PublishOrder(Order order);
        IReadOnlyCollection<Message> CollectOrders();
        Task AddOrdersAsync(IEnumerable<Order> orders);
        Task AddEmployeeOrderAsync(EmployeeOrder employeeOrder);
        Task<Order> GetPendingOrderAsync();
        Task UpdateOrdersAsync(params Order[] orders);
        Task FinishedEmployeeOrdersAsync();
        Task StartEmployeeOrderAsync();
        Task<Order> GetOrderAsync(int userId, string orderNumber);
    }
}
