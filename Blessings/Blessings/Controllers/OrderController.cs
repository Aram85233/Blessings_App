using AutoMapper;
using Blessings.Common.BackgroundWorker.Messaging;
using Blessings.Common.Publisher;
using Blessings.Common.Subscriber.Messaging;
using Blessings.Data.Entities;
using Blessings.Models;
using Blessings.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Blessings.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IMessagesRepository _messagesRepository;
        public OrderController(IOrderService orderService,
                               IMapper mapper,
                               IMessagesRepository messagesRepository)
        {
            _orderService = orderService;
            _mapper = mapper;
            _messagesRepository = messagesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderModel orderModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = _mapper.Map<Order>(orderModel);

            _orderService.PublishOrder(order);

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
