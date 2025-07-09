using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RentCar.Application.DTOs;
using RentCar.Application.Security.AuthEnums;
using RentCar.Application.Services.Interfaces;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [Authorize(Policy = nameof(ApplicationPermissionCode.GetBrand))]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.GetBrand))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _brandService.GetById(id);
            if (brand == null)
            {
                return NotFound($"Brand with ID {id} not found.");
            }
            return Ok(brand);
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.CreateBrand))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBrandDto dto)
        {
            await _brandService.Add(dto);
            return StatusCode(201); // Created
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.UpdateBrand))]
        [HttpPut]
        public IActionResult Update(int id, [FromBody] UpdateBrandDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Brand ID mismatch.");
            }
            _brandService.Update(dto);
            return NoContent(); // 204 No Content
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.DeleteBrand))]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _brandService.Delete(id);
            return NoContent(); // 204 No Content
        }
    }
}
