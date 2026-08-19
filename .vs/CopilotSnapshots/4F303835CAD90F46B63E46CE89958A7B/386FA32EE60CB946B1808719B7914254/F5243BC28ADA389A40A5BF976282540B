using Microsoft.EntityFrameworkCore;
using SmartShip.ShipmentService.Application.DTOs;
using SmartShip.ShipmentService.Application.Interfaces;
using SmartShip.ShipmentService.Domain.Entities;
using SmartShip.ShipmentService.Infrastructure.Data;

namespace SmartShip.ShipmentService.Application.Services
{
    public class ShipmentService: IShipmentService
    {
        private readonly ShipmentDbContext _context;

        public ShipmentService(ShipmentDbContext context)
        {
            _context = context;
        }

        public async Task<Shipment> CreateShipmentAsync(CreateShipmentDto dto, int customerId)
        {
            if (string.IsNullOrWhiteSpace(dto.PackageDescription))
            {
                throw new ArgumentException(
                    "Pacakge description is required");
            }


            if (dto.PackageWeight <= 0)
            {
                throw new ArgumentException(
                    "Package weight must be greater than zero.");
            }

            if (dto.OriginAddressId == dto.DestinationAddressId)
            {
                throw new ArgumentException(
                    "Origin and destination addresses cannot be the same.");
            }
            // address validation to check customer can send to their created start and end address

            var originalAddress = await _context.Addresses
                .FirstOrDefaultAsync(x =>
                x.Id == dto.OriginAddressId && x.CustomerId == customerId);

            if (originalAddress == null)
            {
                throw new ArgumentException(
                    "Invalid origin address");
            }

            // validation
            var destinationAddress = await _context.Addresses
        .FirstOrDefaultAsync(x =>
            x.Id == dto.DestinationAddressId &&
            x.CustomerId == customerId);

            if (destinationAddress == null)
            {
                throw new ArgumentException(
                    "Invalid destination address.");
            }


            var shipment = new Shipment
            {
                CustomerId = customerId,
                OriginAddressId = dto.OriginAddressId,
                DestinationAddressId = dto.DestinationAddressId,
                ShipmentType = dto.ShipmentType,
                Status = ShipmentStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            _context.Shipments.Add(shipment);

            await _context.SaveChangesAsync();

            var package = new Package
            {
                ShipmentId = shipment.Id,
                Description = dto.PackageDescription,
                Weight = dto.PackageWeight
            };

            _context.Packages.Add(package);

            await _context.SaveChangesAsync();

            return shipment;
        }

        public async Task<Shipment?> GetShipmentByIdAsync(int id, int customerId) // ? return type can be of shipment type or null
        {
            return await _context.Shipments.FirstOrDefaultAsync(x=> 
            x.Id == id && x.CustomerId == customerId);
        }

        // only customer can get shipment belongs to them 

        public async Task<List<Shipment>> GetAllShipmentsAsync(int customerId)
        {
            return await _context.Shipments
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();
        }


        public async Task<bool>BookShipmentAsync(int id, int customerId)
        {
            var shipment = await _context.Shipments.FirstOrDefaultAsync(x=>
            x.Id == id && 
            x.CustomerId == customerId);

            if(shipment == null)
            {
                return false;
            }

            shipment.Status = ShipmentStatus.Booked;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateShipmentStatusAsync(int id, ShipmentStatus newStatus)
        {
            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(x => x.Id == id);

            if(shipment == null)
            {
                return false;
            }

            bool isValidTransition = shipment.Status switch
            {
                ShipmentStatus.Draft =>
                newStatus == ShipmentStatus.Booked,

                ShipmentStatus.Booked =>
                 newStatus == ShipmentStatus.PickedUp ||
                 newStatus == ShipmentStatus.Failed,

                ShipmentStatus.PickedUp =>
        newStatus == ShipmentStatus.InTransit ||
        newStatus == ShipmentStatus.Failed,

                ShipmentStatus.InTransit =>
                    newStatus == ShipmentStatus.OutForDelivery ||
                    newStatus == ShipmentStatus.Delayed ||
                    newStatus == ShipmentStatus.Returned ||
                    newStatus == ShipmentStatus.Failed,

                ShipmentStatus.Delayed =>
                    newStatus == ShipmentStatus.InTransit ||
                    newStatus == ShipmentStatus.Returned ||
                    newStatus == ShipmentStatus.Failed,

                ShipmentStatus.OutForDelivery =>
                    newStatus == ShipmentStatus.Delivered ||
                    newStatus == ShipmentStatus.Failed ||
                    newStatus == ShipmentStatus.Returned,

                ShipmentStatus.Delivered =>
                    false,

                ShipmentStatus.Failed =>
                    false,

                ShipmentStatus.Returned =>
                    false,

                _ => false


            };

            if (!isValidTransition)
            {
                return false; // Invalid status transition
            }

            shipment.Status = newStatus;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
