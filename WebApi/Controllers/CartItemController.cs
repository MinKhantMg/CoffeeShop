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
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartItemDto dto)
        {
            int cartItem = await _service.Create(dto);

            if (cartItem > 0)
                return Created("", new { result = (cartItem > 0) });
            else
                return StatusCode(500, "Failed to create cart.");
        }

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
       
        [HttpPut("{id}/quantity")]
        public async Task<IActionResult> UpdateItem(string id, int quantity)
        {
            await _service.Update(id, quantity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            await _service.SoftDelete(id);
            return NoContent();
        }

    }
}
