using System.ComponentModel;
using Application.Dto.CategoryDTO;
using Application.Logic.CategoryService;
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

    public CategoryController(ICategoryService service)
    {
        _service = service;
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
    /// Retrieves all Categories
    /// </summary>
    /// <returns>Return a list of catagories</returns>
 
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
    public async Task<IActionResult> Get(string id)
    {
        Category category = await _service.GetById(id);
        return Ok(new { category });
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
