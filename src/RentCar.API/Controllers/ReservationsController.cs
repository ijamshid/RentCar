using Microsoft.AspNetCore.Mvc;
using RentCar.Application.DTOs;
using RentCar.Application.Services.Interfaces;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // GET: api/reservation
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _reservationService.GetAllAsync();
            return Ok(reservations);
        }

        // GET: api/reservation/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
                return NotFound($"Reservation with ID {id} not found.");

            return Ok(reservation);
        }

        // POST: api/reservation
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservationCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdReservation = await _reservationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdReservation.Id }, createdReservation);
        }

        // PUT: api/reservation/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReservationUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Reservation ID mismatch.");

            var updated = await _reservationService.UpdateAsync(dto);
            if (!updated)
                return NotFound($"Reservation with ID {id} not found.");

            return NoContent();
        }

        // DELETE: api/reservation/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _reservationService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Reservation with ID {id} not found.");

            return NoContent();
        }
    }
}
