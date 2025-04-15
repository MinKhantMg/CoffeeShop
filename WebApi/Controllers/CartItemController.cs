using Application.Dto.CartItemDTO;
using Application.Logic.CartItemService;
using Domain.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _service;

        public CartItemController(ICartItemService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create CartItem
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartItemDto dto)
        {
            if (dto == null)
                return BadRequest("Cart item data is required.");

            try
            {
                var result = await _service.Create(dto);

                if (result is not null)
                    return Created(string.Empty, new { result = true });

                return StatusCode(500, "Failed to create cart item.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartItemsById(string cartId)
        {
            var cartItem = await _service.GetCartItemsByCartIdAsync(cartId);
            if (cartItem == null)
            {
                return NotFound("Cart not found");
            }
            return Ok(cartItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("adjust")]
        public async Task<IActionResult> AdjustQuantity([FromBody] AdjustCartItemDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Id))
                return BadRequest("Invalid request");

            try
            {
                var result = await _service.AdjustQuantityAsync(request.Id, request.Quantity);
                return Ok(result); // or return NoContent() if you don't want to send back the result
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            await _service.SoftDelete(id);
            return NoContent();
        }

    }
}
