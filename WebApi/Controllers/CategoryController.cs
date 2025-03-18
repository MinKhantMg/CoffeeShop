using Application.Dto.CategoryDTO;
using Application.Logic.CategoryService;
using Application.Logic.UserService;
using AutoMapper.Execution;
using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

  
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryAddDto dto)
    {
        var user = HttpContext.User;

        int categoryCreated = await _service.Create(dto,user);

        if (categoryCreated > 0)
            return Created("", new { result = (categoryCreated > 0) });
        else
            return StatusCode(500, "Failed to create category.");
        
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var category = (await _service.GetAll()).ToList();
        int totalItems = await _service.CountAll(); // Note that this can be paginated
        return Ok(new { category, totalItems });
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        Category category = await _service.GetById(id);
        return Ok(new { category });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CategoryAddDto dto)
    {
        int categoryEdited = await _service.Update(id, dto);
        return Ok(new { result = (categoryEdited > 0) });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        bool result = await _service.SoftDelete(id);
        return Ok(new { message = (result) ? "Category record was deleted." : "An error occured." });
    }

}
