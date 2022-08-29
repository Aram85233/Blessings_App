using AutoMapper;
using Blessings.Data.Entities;
using Blessings.Models;
using Blessings.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blessings.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService,
                               IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel orderModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = _mapper.Map<Order>(orderModel);
            order.UserId = int.Parse(HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _orderService.PublishOrder(order);

            return Ok();
        }

        [HttpGet("order/{orderNumber}/status")]
        public async Task<IActionResult> CheckOrderStatus(string orderNumber)
        {
            var userId = int.Parse(HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var order = await _orderService.GetOrderAsync(userId, orderNumber);

            if (order == null)
            {
                ModelState.AddModelError("error", "Order not found");
                return BadRequest(ModelState);
            }

            return Ok(order);
        }
    }
}
