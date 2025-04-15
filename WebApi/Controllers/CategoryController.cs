using System.ComponentModel;
using Application.Dto.CategoryDTO;
using Application.Logic.CategoryService;
using Application.Logic.ProductService;
using Application.Logic.ProductVariantService;
using Application.Logic.SubCategoryService;
using Application.Logic.UserService;
using AutoMapper.Execution;
using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;
    private readonly ISubCategoryService _subCategoryService;
    private readonly IProductService _productService;
    private readonly IProductVariantService _productVariantService;

    public CategoryController(ICategoryService service, ISubCategoryService subCategoryService, IProductService productService, IProductVariantService productVariantService)
    {
        _service = service;
        _subCategoryService = subCategoryService;
        _productService = productService;
        _productVariantService = productVariantService;
    }

    /// <summary>
    /// Create Category
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryAddDto dto)
    {
        var user = HttpContext.User;

        int categoryCreated = await _service.Create(dto, user);

        if (categoryCreated > 0)
            return Created("", new { result = (categoryCreated > 0) });
        else
            return StatusCode(500, "Failed to create category.");

    }

    /// <summary>
    /// Select the Category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("select")]
    public IActionResult SelectCategory(string id)
    {
        if (id == null )
            return BadRequest("Invalid category ID.");

        // You can store the category ID in session, local storage, or return it in response
        return Ok(new { result = id});
    }

    /// <summary>
    /// Retrieves all Categories
    /// </summary>
    /// <returns>Return a list of catagories</returns>
    [AllowAnonymous]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var category = (await _service.GetAll()).ToList();
        int totalItems = await _service.CountAll();
        return Ok(new { category, totalItems });
    }

    /// <summary>
    /// Get Category By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<Category> Get(string id)
    {
        Category category = await _service.GetById(id);
        return  category;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetAdminSummary()
    {
        var categoryCount = await _service.CountAll();
        var subcategoryCount = await _subCategoryService.CountAll();
        var productCount = await _productService.CountAll();
        var productVariantCount = await _productVariantService.CountAll();

        var summary = new
        {
            Categories = categoryCount,
            Subcategories = subcategoryCount,
            Products = productCount,
            ProductVariants = productVariantCount
        };

        return Ok(summary);
    }


    /// <summary>
    /// Update Category By Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CategoryAddDto dto)
    {
        var user = HttpContext.User;
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Category name is required");
        }

        int categoryEdited = await _service.Update(id, dto, user);
        return Ok(new { result = (categoryEdited > 0) });
    }

    /// <summary>
    /// Delete Category By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      //var user = HttpContext.User;
        int result = await _service.SoftDelete(id);
        return Ok(new { result = (result > 0) });
    }

}
