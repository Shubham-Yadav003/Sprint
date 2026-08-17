using SmartShip.AdminService.Application.DTOs;

namespace SmartShip.AdminService.Application.Interfaces
{
    public interface IShipmentManagementService
    {
        Task<(bool Success, string Message)> UpdateShipmentProgressAsync(int shipmentId, UpdateShipmentProgressDto dto);
    }
}
