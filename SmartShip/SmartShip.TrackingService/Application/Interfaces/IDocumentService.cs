using SmartShip.TrackingService.Application.DTOs;

namespace SmartShip.TrackingService.Application.Interfaces
{
    public interface IDocumentService
    {
        Task<ShipmentDocumentDto> CreateDocumentAsync(
           CreateShipmentDocumentDto dto);

        Task<List<ShipmentDocumentDto>> GetDocumentsByShipmentIdAsync(
            int shipmentId);
    }
}
