using Microsoft.AspNetCore.Mvc;
using RentCar.Application.DTOs;
using RentCar.Application.Services.Interfaces;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        // GET: api/rating
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ratings = await _ratingService.GetAllAsync();
            return Ok(ratings);
        }

        // GET: api/rating/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rating = await _ratingService.GetByIdAsync(id);
            if (rating == null)
                return NotFound($"Rating with ID {id} not found.");

            return Ok(rating);
        }

        // POST: api/rating
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RatingCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdRating = await _ratingService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdRating.Id }, createdRating);
        }

        // PUT: api/rating/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RatingUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Rating ID mismatch.");

            var updated = await _ratingService.UpdateAsync(dto);
            if (!updated)
                return NotFound($"Rating with ID {id} not found.");

            return NoContent();
        }

        // DELETE: api/rating/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _ratingService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Rating with ID {id} not found.");

            return NoContent();
        }
    }
}
