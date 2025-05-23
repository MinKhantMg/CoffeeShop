﻿using Application.Dto.ProductDTO;
using Application.Dto.ProductVariantDTO;
using Application.Logic.ProductVariantService;
using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly IProductVariantService _service;

        public ProductVariantController(IProductVariantService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create ProductVariant By ProductId
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductVariantDto dto)
        {
            var user = HttpContext.User;
            int productVariantCreated = await _service.Create(dto, user);

            if (productVariantCreated > 0)
                return Created("", new { result = (productVariantCreated > 0) });
            else
                return StatusCode(500, "Failed to create productvariant.");
        }

        /// <summary>
        /// Retrieves all ProductVariants
        /// </summary>
        /// <returns>Return a list of ProductVariants</returns>
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var productVariant = (await _service.GetAll()).ToList();
                int totalItems = await _service.CountAll();

                // Ensure ImageUrl is correctly mapped (for Blazor UI)
                foreach (var pv in productVariant)
                {
                    if (!string.IsNullOrEmpty(pv.ImageUrl) && !pv.ImageUrl.StartsWith("http"))
                        pv.ImageUrl = $"/uploads/{pv.ImageUrl}";
                }

                return Ok(new { productVariant, totalItems });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetAll ProductVariants: " + ex.Message);
                return StatusCode(500, "Server error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get ProductVariant By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProductVariant> Get(string id)
        {
            ProductVariant productVariant = await _service.GetById(id);
            return productVariant;
        }

        /// <summary>
        /// Get ProductVariant By ProductId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("all/{productId}")]
        public async Task<IEnumerable<ProductVariant>> GetByCategoryId(string productId)
        {
            var productVariants = await _service.GetByProductId(productId);
            return productVariants;
        }

        /// <summary>
        /// Update ProductVariant By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ProductVariantDto dto)
        {
            var user = HttpContext.User;
            int productVariantEdited = await _service.Update(id, dto, user);
            return Ok(new { result = (productVariantEdited > 0) });
        
        }

        /// <summary>
        /// Delete ProductVariant By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {

            int result = await _service.SoftDelete(id);
            return Ok(new { result = (result > 0) });
        }

    }
}
