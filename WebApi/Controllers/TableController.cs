using Application.Dto.CategoryDTO;
using Application.Dto.TableDTO;
using Application.Logic.TableService;
using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _service;

        public TableController(ITableService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create Table
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TableDto dto)
        {
            var user = HttpContext.User;

            int tableCreated = await _service.Create(dto, user);

            if (tableCreated > 0)
                return Created("", new { result = (tableCreated > 0) });
            else
                return StatusCode(500, "Failed to create table.");
        }


        /// <summary>
        /// Retrieves all Tables
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
        /// Get Table By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Table table = await _service.GetById(id);
            return Ok(new { table });
        }

        /// <summary>
        /// Update Table By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TableDto dto)
        {
            var user = HttpContext.User;
            int tableEdited = await _service.Update(id, dto, user);
            return Ok(new { result = (tableEdited > 0) });
        }

        /// <summary>
        /// Delete Table By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = HttpContext.User;
            bool result = await _service.SoftDelete(id, user);
            return Ok(new { message = (result) ? "Table record was deleted." : "An error occured." });
        }
    }
}
