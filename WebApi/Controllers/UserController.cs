using Application.Dto;
using Application.Dto.UserDTO;
using Application.Logic.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="userRegistrationDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer([FromBody] UserRegistrationDto userRegistrationDto)
        {
            int response = await _service.RegisterUserAsync(userRegistrationDto);
            return Created("", new { result = (response > 0) });

        }


    }
}
