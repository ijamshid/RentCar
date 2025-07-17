using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentCar.Application.Models.Car;
using RentCar.Application.Models.Reservation;
using RentCar.Application.Security.AuthEnums;
using RentCar.Application.Services.Interfaces;
using System.Security.Claims;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Foydalanuvchi autentifikatsiyadan o‘tgan bo‘lishi shart
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.CreateReservation))]
        // POST: api/reservations
        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            try
            {
                var createdReservation = await _reservationService.CreateReservationAsync(dto, userId);
                return CreatedAtAction(nameof(GetReservationById), new { id = createdReservation.Id }, createdReservation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [Authorize(Policy = nameof(ApplicationPermissionCode.UpdateReservation))]
        // POST: api/reservations
        [HttpPut]
        public async Task<IActionResult> UpdateReservation([FromBody] ReservationUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var Reservation = await _reservationService.UpdateAsync(dto);
            return Ok(Reservation);
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.CancelReservation))]
        // POST: api/reservations/cancel/{id}
        [HttpPost("cancel/{id}")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _reservationService.CancelReservationAsync(id, userId);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        // POST: api/reservations/return/{id}
        [HttpPost("return/{id}")]
        public async Task<IActionResult> ReturnCar(int id, [FromBody] ReturnCarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _reservationService.ReturnCarAsync(id, dto, userId);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [Authorize(Policy = nameof(ApplicationPermissionCode.GetReservation))]
        // Qo‘shimcha: GetById endpointi CreatedAtAction uchun
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }
        [Authorize(Policy = nameof(ApplicationPermissionCode.GetReservation))]
        // Qo‘shimcha: GetById endpointi CreatedAtAction uchun
        [HttpGet]
        public async Task<IActionResult> GetReservation()
        {
            var reservation = await _reservationService.GetAllAsync();
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }
        [Authorize(Policy = nameof(ApplicationPermissionCode.DeleteReservation))]
        // Qo‘shimcha: GetById endpointi CreatedAtAction uchun
        [HttpDelete]
        public async Task<IActionResult> DeleteReservation(int id)
        {            
            var reservation = await _reservationService.DeleteAsync(id);

            return NoContent();
        }

        // Token ichidan userId olish uchun yordamchi metod
        private string? GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
