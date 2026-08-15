using SmartShip.TrackingService.Application.DTOs;

namespace SmartShip.TrackingService.Application.Interfaces
{
    public interface IDocumentService
    {
        Task<ShipmentDocumentDto> UploadDocumentAsync(int shipmentId, string documentType, IFormFile file);
           

        Task<List<ShipmentDocumentDto>> GetDocumentsByShipmentIdAsync(
            int shipmentId);
    }
}
