using Blessings.Services.Contracts;

namespace Blessings.Services.Impl
{
    public class OrderService : IOrderService
    {
        public void GetOrders()
        {
            Console.WriteLine("Get orders!");
        }
    }
}
