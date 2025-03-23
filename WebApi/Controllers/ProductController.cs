using Application.Dto.ProductDTO;
using Application.Dto.SubCategoryDTO;
using Application.Logic.ProductService;
using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create Product By CategoryId and SubCategoryId
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductAddDto dto)
        {
            var user = HttpContext.User;

            int productCreated = await _service.Create(dto, user);

            if (productCreated > 0)
                return Created("", new { result = (productCreated > 0) });
            else
                return StatusCode(500, "Failed to create product.");
        }


        /// <summary>
        /// Retrieves all Products
        /// </summary>
        /// <returns>Return a list of Sub Catagories</returns>
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var product = (await _service.GetAll()).ToList();
            int totalItems = await _service.CountAll(); // Note that this can be paginated
            return Ok(new { product, totalItems });
        }

        /// <summary>
        /// Get Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Product product = await _service.GetById(id);
            return Ok(new { product });
        }

        /// <summary>
        /// Update Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ProductAddDto dto)
        {
            var user = HttpContext.User;
            int productEdited = await _service.Update(id, dto, user);
            return Ok(new { result = (productEdited > 0) });
        }

        /// <summary>
        /// Delete Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = HttpContext.User;
            bool result = await _service.SoftDelete(id, user);
            return Ok(new { message = (result) ? "Product record was deleted." : "An error occured." });
        }

    }
}
