using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCar.Application.DTOs;
using RentCar.Application.Services.Interfaces;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController(ICarService carService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cars = await carService.GetAllAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var cars = await carService.GetByIdAsync(id);
            return Ok(cars);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CarCreateDto dto)
        {
            await carService.CreateAsync(dto);
            return Created();
        }

        [HttpPut]
        public IActionResult Update([FromBody] CarUpdateDto dto)
        {
            carService.Update(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            carService.Delete(id);
            return NoContent();
        }
    }
}