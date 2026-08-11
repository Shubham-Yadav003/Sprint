using SmartShip.ShipmentService.Domain.Entities;

namespace SmartShip.ShipmentService.Application.DTOs
{
    public class UpdateShipmentStatusDto
    {
        public ShipmentStatus Status { get; set; }
    }
}
