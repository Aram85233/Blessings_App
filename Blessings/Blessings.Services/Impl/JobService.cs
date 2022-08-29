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
                var aaa = JsonSerializer.Deserialize<Order>(order.Text);
                var result = new Order
                {
                    CreatedDate = order.CreationDate,
                    OrderNumber = order.Id,
                    Quantity = aaa.Quantity,
                    SetId = aaa.SetId

                };
                ordersForInsert.Add(result);
            }   
            await _orderService.AddOrdersAsync(ordersForInsert);
        }
    }
}
