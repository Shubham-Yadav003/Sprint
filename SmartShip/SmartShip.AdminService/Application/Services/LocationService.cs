using Microsoft.EntityFrameworkCore;
using SmartShip.AdminService.Application.DTOs;
using SmartShip.AdminService.Application.Interfaces;
using SmartShip.AdminService.Domain.Entities;
using SmartShip.AdminService.Infrastructure.Data;

namespace SmartShip.AdminService.Application.Services
{
    public class LocationService: ILocationService
    {
        private readonly AdminDbContext _context;

        public LocationService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LocationDto>> GetAllLocationsAsync()
        {
            return await _context.Locations
                .Select(l => new LocationDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    Address = l.Address,
                    City = l.City,
                    IsActive = l.IsActive
                })
                .ToListAsync();

        }

        public async Task<LocationDto?> GetLocationByIdAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return null;

            return new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
                City = location.City,
                IsActive = location.IsActive
            };
        }

        public async Task<LocationDto> CreateLocationAsync(CreateLocationDto dto)
        {
            var location = new Location
            {
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                IsActive = true
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
                City = location.City,
                IsActive = location.IsActive
            };
        }

        public async Task<bool> UpdateLocationAsync(int id, UpdateLocationDto dto)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return false;

            location.Name = dto.Name;
            location.Address = dto.Address;
            location.City = dto.City;
            location.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleLocationStatusAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return false;

            location.IsActive = !location.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
