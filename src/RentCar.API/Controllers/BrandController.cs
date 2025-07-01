using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RentCar.Application.DTOs;
using RentCar.Application.Services;

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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAll();
            return Ok(brands);
        }

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

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBrandDto dto)
        {
            await _brandService.Add(dto);
            return StatusCode(201); // Created
        }

        [HttpPut]
        public IActionResult Update( int id, [FromBody] UpdateBrandDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Brand ID mismatch.");
            }
            _brandService.Update(dto);
            return NoContent(); // 204 No Content
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _brandService.Delete(id);
            return NoContent(); // 204 No Content
        }
    }
}
