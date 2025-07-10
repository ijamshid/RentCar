using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentCar.Application.Models.Photo;
using RentCar.Application.Security.AuthEnums;
using RentCar.Application.Services;

namespace RentCar.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PhotosController(IPhotoService service) : ControllerBase
{

    [Authorize(Policy = nameof(ApplicationPermissionCode.GetPhoto))]
    [HttpGet]
    public async Task<IActionResult> GetAllPhotos()
    {
        return Ok(await service.GetPhotosAsync());
    }

    [Authorize(Policy = nameof(ApplicationPermissionCode.GetPhoto))]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdPhoto(int id)
    {
        return Ok(service.GetPhotoBtCarIdAsync(id));
    }



    [Authorize(Policy = nameof(ApplicationPermissionCode.CreatePhoto))]
    [HttpPost]
    public async Task<IActionResult> CreatePhoto([FromBody] CreatePhotoModel request)
    {
        var res = await service.CreatePhotoAsync(request);

        if (!res.IsSuccess)
        {
            return BadRequest();
        }

        return Ok(res);
    }


    [Authorize(Policy = nameof(ApplicationPermissionCode.DeletePhoto))]
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteByIdPhoto(int id)
    {
        var res = await service.DeletePhotoAsync(id);

        if (!res.IsSuccess)
        {
            return NotFound();
        }

        return Ok(res);

    }


}
