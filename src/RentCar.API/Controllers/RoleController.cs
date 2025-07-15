using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentCar.Application.Security.AuthEnums;
using RentCar.Application.Services.Interfaces;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRoleService roleService) : ControllerBase
    {
        [Authorize(Policy = nameof(ApplicationPermissionCode.GetRole))]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await roleService.GetAllAsync();
            return Ok(roles);
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.GetRole))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await roleService.GetByIdAsync(id);
            if (role is null) return NotFound();
            return Ok(role);
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.CreateRole))]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string name)
        {
            await roleService.CreateAsync(name);
            return Created();
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.UpdateRole))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] string newName)
        {
            await roleService.UpdateAsync(id, newName);
            return NoContent();
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.DeleteRole))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await roleService.DeleteAsync(id);
            return NoContent();
        }
    }

}
