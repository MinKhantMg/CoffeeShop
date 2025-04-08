using Application.Dto.OrderDTO;
using Application.Logic.OrderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetOrderSummary([FromQuery] string cartId)
        {
            var orderSummary = await _orderService.GetOrderSummary(cartId);
            return Ok(orderSummary);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var order = await _orderService.CreateOrder(dto.CartId, dto.PaymentType);
            return Ok(order);
        }
    }
}
