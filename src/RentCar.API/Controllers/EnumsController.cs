using Microsoft.AspNetCore.Mvc;
using RentCar.Application.DTOs;
using RentCar.Core.Enums;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<SelectListItemDto>> GetEnumSelectList([FromQuery] string enumName)
        {
            Type? enumType = enumName switch
            {
                "FuelType" => typeof(FuelType),
                "PaymentSatus" => typeof(PaymentStatus),
                "ReservationStatus" => typeof(ReservationStatus),
                _ => null
            };

            if(enumType == null || !enumType.IsEnum)
            {
                return BadRequest("Invalid enum name provided.");
            }

            var list = Enum.GetValues(enumType)
                .Cast<object>()
                .Select(e => new SelectListItemDto
                {
                    Value = (int)e,
                    Text = e.ToString() ?? ""
                }).ToList();

            return Ok(list);
        }

    }
}
