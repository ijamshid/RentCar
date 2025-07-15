using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentCar.Application.Models.User2;
using RentCar.Application.Security.AuthEnums;
using RentCar.Application.Services.Interfaces;
namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.UserRead))]
        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.UserRead))]
        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }


        [Authorize(Policy = nameof(ApplicationPermissionCode.UserRead))]
        [HttpGet("get-user-auth")]
        public async Task<IActionResult> GetUserAuth()
        {
            var result = await _userService.GetUserAuth();
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.UserCreate))]
        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            var createdUser = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.UserUpdate))]
        // PUT: api/User
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto dto)
        {
            var isUpdated = await _userService.UpdateAsync(dto);
            if (!isUpdated)
                return NotFound($"User with ID {dto.Id} not found.");

            return Ok("User successfully updated.");
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.UserDelete))]
        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _userService.DeleteAsync(id);
            if (!isDeleted)
                return NotFound($"User with ID {id} not found.");

            return Ok("User successfully deleted.");
        }


    }

    
}
