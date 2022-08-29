using Blessings.Common.BackgroundWorker.Messaging;
using Blessings.Common.Publisher;
using Blessings.Common.Subscriber.Messaging;
using Blessings.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Blessings.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRabbitPublisher _publisher;
        private readonly IMessagesRepository _messagesRepository;
        public OrderController(IRabbitPublisher publisher,
                               IMessagesRepository messagesRepository)
        {
            _publisher = publisher;
            _messagesRepository = messagesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderModel orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var message = new Message()
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                Text = JsonSerializer.Serialize(orderDto)
            };

            _publisher.Publish(message);

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var messages = _messagesRepository.GetMessages();
            return Ok(messages);
        }
    }
}
