using SmartShip.ShipmentService.Application.DTOs;
using SmartShip.ShipmentService.Application.Interfaces;
using SmartShip.ShipmentService.Domain.Entities;
using SmartShip.ShipmentService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SmartShip.ShipmentService.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly ShipmentDbContext _context;

        public AddressService(ShipmentDbContext context)
        {
            _context = context;
        }

        public async Task<Address> CreateAddressAsync(
            CreateAddressDto dto,
            int customerId)
        {
            var address = new Address
            {
                CustomerId = customerId,
                Addressline = dto.Addressline,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode
            };

            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();

            return address;
        }

        public async Task<List<Address>> GetAddressesAsync(int customerId)
        {
            return await _context.Addresses
                .Where(address => address.CustomerId == customerId)
                .OrderBy(address => address.Id)
                .ToListAsync();
        }
    }
}
