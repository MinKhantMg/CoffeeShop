using Application.Dto.CategoryDTO;
using Application.Dto.SubCategoryDTO;
using Application.Logic.SubCategoryService;
using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _service;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _service = subCategoryService;
        }

        /// <summary>
        /// Create SubCategory By CategoryId
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubCategoryDto dto)
        {
            var user = HttpContext.User;

            int subCategoryCreated = await _service.Create(dto, user);

            if (subCategoryCreated > 0)
                return Created("", new { result = (subCategoryCreated > 0) });
            else
                return StatusCode(500, "Failed to create subcategory.");
        }


        /// <summary>
        /// Retrieves all SubCategories
        /// </summary>
        /// <returns>Return a list of Sub Catagories</returns>
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var subCategory = (await _service.GetAll()).ToList();
            int totalItems = await _service.CountAll(); // Note that this can be paginated
            return Ok(new { subCategory, totalItems });
        }

        /// <summary>
        /// Get SubCategory By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            SubCategory subCategory = await _service.GetById(id);
            return Ok(new { subCategory });
        }

        /// <summary>
        /// Get SubCategory By CategoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("all/{categoryId}")]
        public async Task<IEnumerable<SubCategory>> GetByCategoryId(string categoryId)
        {
            var subCategory = await _service.GetByCategoryId(categoryId);
            return subCategory;
        }

        /// <summary>
        /// Update SubCategory By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] SubCategoryDto dto)
        {
            var user = HttpContext.User;
            int categoryEdited = await _service.Update(id, dto, user);
            return Ok(new { result = (categoryEdited > 0) });
        }

        /// <summary>
        /// Delete SubCategory By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
           // var user = HttpContext.User;
            int result = await _service.SoftDelete(id);
            return Ok(new { result = (result > 0) });
        }

    }
}
