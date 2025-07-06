using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentCar.Application.DTOs;
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

        // POST: api/reservations
        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var createdReservation = await _reservationService.CreateReservationAsync(dto, userId);
            return CreatedAtAction(nameof(GetReservationById), new { id = createdReservation.Id }, createdReservation);
        }

        // POST: api/reservations/confirm/{id}
        [HttpPost("confirm/{id}")]
        public async Task<IActionResult> ConfirmReservation(int id)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _reservationService.ConfirmReservationAsync(id, userId);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

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

        // Qo‘shimcha: GetById endpointi CreatedAtAction uchun
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        // Token ichidan userId olish uchun yordamchi metod
        private string? GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
