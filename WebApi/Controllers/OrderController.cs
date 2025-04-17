using Application.Dto.OrderDTO;
using Application.Logic;
using Application.Logic.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly PdfService _pdfService;

        public OrderController(IOrderService orderService, PdfService pdfService)
        {
            _orderService = orderService;
            _pdfService = pdfService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var order = await _orderService.CreateOrder(dto.CartId, dto.PaymentType, dto.OrderType);
            return Ok(order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("preview-summary")]
        public async Task<IActionResult> PreviewOrderSummary([FromBody] CreateOrderDto dto)
        {
            var cartItems = await _orderService.GetCartItems(dto.CartId);
            if (cartItems == null || !cartItems.Any())
                return NotFound("Cart is empty.");

            var preview = new OrderSummaryDto
            {
                OrderType = dto.OrderType,
                PaymentType = dto.PaymentType,
                TotalAmount = cartItems.Sum(i => i.Price * i.Quantity),
                Items = cartItems.Select(i => new OrderItemDto
                {
                    ProductVariantName = i.ProductVariantName,
                    Quantity = i.Quantity,
                    SubTotal = i.Price * i.Quantity
                }).ToList()
            };

            return Ok(preview);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("generate-receipt/{orderId}")]
        public async Task<IActionResult> GenerateOrderReceipt(string orderId)
        {
            var orderSummary = await _orderService.GetOrderSummaryByOrderId(orderId);
            var fileName = "Café de Luxe Receipt.pdf";

            if (orderSummary == null)
            {
                return NotFound("Order not found");
            }

            var pdfBytes = _pdfService.GenerateOrderReceiptPdf(orderSummary);
            return File(pdfBytes, "application/pdf", fileName);
        }


        [Authorize(Roles = "Staff")]
        [HttpPut("confirm/{orderId}")]
        public async Task<IActionResult> ConfirmOrder(string orderId)
        {
            try
            {
                var result = await _orderService.ConfirmOrder(orderId);
                if (result)
                    return Ok(new { message = "Order confirmed successfully." });

                return BadRequest(new { message = "Failed to confirm order." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingOrder()
        {
            try
            {
                var pendingOrder = await _orderService.GetPendingOrdersAsync();

                if (pendingOrder == null)
                {
                    return NotFound("Pending order not found.");
                }

                return Ok(pendingOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> GetConfirmOrder()
        {
            try
            {
                var pendingOrder = await _orderService.GetConfirmOrdersAsync();

                if (pendingOrder == null)
                {
                    return NotFound("Confirm order not found.");
                }

                return Ok(pendingOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


    }
}
