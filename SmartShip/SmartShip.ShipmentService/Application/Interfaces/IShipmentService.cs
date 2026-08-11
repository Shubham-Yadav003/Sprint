using SmartShip.ShipmentService.Domain.Entities;
using SmartShip.ShipmentService.Application.DTOs;

namespace SmartShip.ShipmentService.Application.Interfaces
{
    public interface IShipmentService
    {
        Task<Shipment> CreateShipmentAsync(CreateShipmentDto dto, int customerId);
        Task<Shipment> GetShipmentByIdAsync(int id, int customerId);

        Task<List<Shipment>> GetAllShipmentsAsync(int customerId);

        Task<bool> BookShipmentAsync(int id, int customerId);

        Task<bool> UpdateShipmentStatusAsync(int id, ShipmentStatus status);

    }
}
