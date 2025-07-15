using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCar.Application.Models.Car;
using RentCar.Application.Security.AuthEnums;
using RentCar.Application.Services;
using RentCar.Application.Services.Interfaces;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController(ICarService carService, IFileStorageService storageService) : ControllerBase
    {
       

        [Authorize(Policy = nameof(ApplicationPermissionCode.GetCar))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var cars = await carService.GetByIdAsync(id);
            return Ok(cars);
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.GetCar))]
        [HttpGet("by-brand/{brand}")]
        public async Task<IActionResult> GetByBrand([FromRoute] string brand)
        {
            var cars = await carService.GetByBrand(brand);
            return Ok(cars);
        }
        [Authorize(Policy = nameof(ApplicationPermissionCode.GetCar))]
        [HttpGet]
        public async Task<IActionResult> GetlAll()
        {
            var cars = await carService.GetAllAsync();
            return Ok(cars);
        }

        [HttpGet("photos/{objectName}")]
        public async Task<IActionResult> GetPhoto([FromRoute] string objectName)
        {
            string bucket = "car-photos";

            var stream = await storageService.DownloadFileAsync(bucket, objectName);
            if (stream == null)
                return NotFound();

            return File(stream, "application/octet-stream", objectName);
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.CreateCar))]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CarCreateDto dto)
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