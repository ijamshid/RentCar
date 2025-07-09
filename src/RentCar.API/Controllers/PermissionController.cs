using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentCar.Application.Security.AuthEnums;
using RentCar.Application.Services;

namespace RentCar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        [Authorize(Policy = nameof(ApplicationPermissionCode.GetPermissions))]
        public IActionResult GetPermissions()
        {
            var allPermissions = _permissionService.GetAllPermissionDescriptions();
            return Ok(allPermissions);
        }

        [HttpGet("all-grouped")]
        [Authorize(Policy = nameof(ApplicationPermissionCode.GetPermissions))]
        public async Task<IActionResult> GetGroupedPermissionsFromDb()
        {
            var result = await _permissionService.GetPermissionsFromDbAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
