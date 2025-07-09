using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCar.Application.DTOs;
using RentCar.Application.Security.AuthEnums;
using RentCar.Application.Services.Interfaces;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController(ICarService carService) : ControllerBase
    {
       

        [Authorize(Policy = nameof(ApplicationPermissionCode.GetCar))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var cars = await carService.GetByIdAsync(id);
            return Ok(cars);
        }
        [Authorize(Policy = nameof(ApplicationPermissionCode.GetCar))]
        [HttpGet]
        public async Task<IActionResult> GetlAll()
        {
            var cars = await carService.GetAllAsync();
            return Ok(cars);
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.CreateCar))]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CarCreateDto dto)
        {
            await carService.CreateAsync(dto);
            return Created();
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.UpdateCar))]
        [HttpPut]
        public IActionResult Update([FromBody] CarUpdateDto dto)
        {
            carService.Update(dto);
            return NoContent();
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.DeleteCar))]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            carService.Delete(id);
            return NoContent();
        }
    }
}