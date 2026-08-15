using SmartShip.TrackingService.Application.DTOs;

namespace SmartShip.TrackingService.Application.Interfaces
{
    public interface IDeliveryProofService
    {
        Task<DeliveryProofDto> UploadDeliveryProofAsync(
            int shipmentId,
            string proofType,
            IFormFile file);

        Task<List<DeliveryProofDto>> GetProofsByShipmentIdAsync(
            int shipmentId);
    }
}
