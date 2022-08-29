using Blessings.Common.BackgroundWorker.Messaging;
using Blessings.Common.Publisher;
using Blessings.Common.Subscriber.Messaging;
using Blessings.Data;
using Blessings.Data.Entities;
using Blessings.Services.Contracts;
using System.Text.Json;

namespace Blessings.Services.Impl
{
    public class OrderService :IOrderService
    {
        private readonly IRabbitPublisher _publisher;
        private readonly IMessagesRepository _messagesRepository;
        public ApplicationDbContext _context { get; set; }
        public OrderService(IRabbitPublisher publisher, 
                            IMessagesRepository messagesRepository, 
                            ApplicationDbContext context)
        {
            _publisher = publisher;
            _messagesRepository = messagesRepository;
            _context = context;
        }

        public void PublishOrder(Order order)
        {
            var message = new Message()
            {
                Id = order.OrderNumber,
                CreationDate = DateTime.UtcNow,
                Text = JsonSerializer.Serialize(order)
            };

            _publisher.Publish(message);
        }

        public IReadOnlyCollection<Message> CollectOrders()
        {
            return _messagesRepository.GetMessages();
        }

        public async Task AddOrdersAsync(IEnumerable<Order> orders)
        {
            _context.Orders.AddRange(orders);
            await _context.SaveChangesAsync();
        }
    }
}
