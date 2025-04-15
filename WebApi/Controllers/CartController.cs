using Application.Dto.CartDTO;
using Application.Logic.CartService;
using Domain.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> CreateCartAsync([FromBody] CartDto cart)
        {
            var cartCreated = await _service.Create(cart);

            if (cartCreated != null)
                return Created("", cartCreated);
            else
                return StatusCode(500, "Failed to create cart.");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartAsync(string cartId)
        {
            var cart = await _service.GetById(cartId);
            if (cart == null)
            {
                return NotFound("Cart not found");
            }
            return Ok(cart);
        }


        //[HttpPut("{cartId}")]
        //public async Task<IActionResult> UpdateCartAsync(string cartId, [FromBody] Cart cart)
        //{

        //    if (cart.Id != cartId)
        //    {
        //        return BadRequest("Cart ID mismatch");
        //    }

        //    await _service.Update(cartId, cart);
        //    return NoContent();
        //}

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> RemoveCartAsync(string cartId)
        {
            await _service.SoftDelete(cartId);
            return NoContent();
        }
    }
}
