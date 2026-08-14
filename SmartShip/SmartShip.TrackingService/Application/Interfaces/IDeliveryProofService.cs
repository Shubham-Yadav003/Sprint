using SmartShip.TrackingService.Application.DTOs;

namespace SmartShip.TrackingService.Application.Interfaces
{
    public interface IDeliveryProofService
    {
        Task<DeliveryProofDto> CreateDeliveryProofAsync(
           CreateDeliveryProofDto dto);

        Task<List<DeliveryProofDto>> GetProofsByShipmentIdAsync(
            int shipmentId);
    }
}
