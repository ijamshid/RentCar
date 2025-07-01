using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentCar.Application.DTOs;
using RentCar.Application.Services.Interfaces;
using RentCar.Core.Entities;
using RentCar.DataAccess.Persistence;

namespace RentCar.Application.Services
{

    public class ReservationService : IReservationService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public ReservationService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReservationGetDto>> GetAllAsync()
        {
            var reservations = await _context.Reservations.ToListAsync();
            return _mapper.Map<IEnumerable<ReservationGetDto>>(reservations);
        }

        public async Task<ReservationGetDto> GetByIdAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            return reservation == null ? null : _mapper.Map<ReservationGetDto>(reservation);
        }

        public async Task<ReservationGetDto> CreateAsync(ReservationCreateDto dto)
        {
            var reservation = _mapper.Map<Reservation>(dto);
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return _mapper.Map<ReservationGetDto>(reservation);
        }

        public async Task<bool> UpdateAsync(ReservationUpdateDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(dto.Id);
            if (reservation == null)
                return false;

            _mapper.Map(dto, reservation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
