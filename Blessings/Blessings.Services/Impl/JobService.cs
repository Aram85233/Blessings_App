using Blessings.Common;
using Blessings.Data.Entities;
using Blessings.Services.Contracts;
using System.Text.Json;

namespace Blessings.Services.Impl
{
    public class JobService : IJobService
    {
        private readonly IOrderService _orderService;
        public JobService(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task GetOrders()
        {
            var ordersForInsert = new List<Order>();

            var orders = _orderService.CollectOrders();
            foreach (var order in orders)
            {
                var orderFromQueue = JsonSerializer.Deserialize<Order>(order.Text);

                if (await _orderService.GetOrderAsync(orderFromQueue.UserId, order.Id) != null)
                    continue;

                var result = new Order
                {
                    CreatedDate = order.CreationDate,
                    OrderNumber = order.Id,
                    Quantity = orderFromQueue.Quantity,
                    SetId = orderFromQueue.SetId,
                    Status = (short)OrderStatus.Pending,
                    UserId = orderFromQueue.UserId

                };
                ordersForInsert.Add(result);
            }
            if (ordersForInsert.Any())
            {
                await _orderService.AddOrdersAsync(ordersForInsert);
            }
        }

        public async Task ProcessOrders()
        {
            await _orderService.FinishedEmployeeOrdersAsync();

            await _orderService.StartEmployeeOrderAsync();

        }
    }
}
