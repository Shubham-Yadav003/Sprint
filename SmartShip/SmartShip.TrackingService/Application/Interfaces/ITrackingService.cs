using SmartShip.TrackingService.Application.DTOs;
namespace SmartShip.TrackingService.Application.Interfaces
{
    public interface ITrackingService
    {
        Task<TrackingEventDto> CreateTrackingEventAsync(CreateTrackingEventDto dto);

        Task<List<TrackingEventDto>> GetTrackingEventsByShipmentIdAsync(int shipmentId);
    }
}
