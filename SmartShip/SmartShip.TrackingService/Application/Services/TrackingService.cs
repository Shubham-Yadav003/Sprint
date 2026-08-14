using Microsoft.EntityFrameworkCore;
using SmartShip.TrackingService.Application.DTOs;
using SmartShip.TrackingService.Application.Interfaces;
using SmartShip.TrackingService.Domain.Entities;
using SmartShip.TrackingService.Infrastructure.Data;
namespace SmartShip.TrackingService.Application.Services
{
    public class TrackingService: ITrackingService
    {
        private readonly TrackingDbContext _context;

        public TrackingService(TrackingDbContext context)
        {
            _context = context;
        }

        public async Task<TrackingEventDto> CreateTrackingEventAsync(CreateTrackingEventDto dto)
        {
            var trackingEvent = new TrackingEvent
            {
                ShipmentId = dto.ShipmentId,
                Status = dto.Status,
                Location = dto.Location,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.TrackingEvents.Add(trackingEvent);

            await _context.SaveChangesAsync();

            return new TrackingEventDto
            {
                Id = trackingEvent.Id,
                ShipmentId = trackingEvent.ShipmentId,
                Status = trackingEvent.Status,
                Location = trackingEvent.Location,
                Description = trackingEvent.Description,
                CreatedAt = trackingEvent.CreatedAt
            };
        }

        public async Task<List<TrackingEventDto>> GetTrackingEventsByShipmentIdAsync(int shipmentId)
        {
            return await _context.TrackingEvents.
                Where(x => x.ShipmentId == shipmentId)
                .OrderBy(x => x.CreatedAt)
                .Select(x => new TrackingEventDto
                {
                    Id = x.Id,
                    ShipmentId = x.ShipmentId,
                    Status = x.Status,
                    Location = x.Location,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }


    }
}
