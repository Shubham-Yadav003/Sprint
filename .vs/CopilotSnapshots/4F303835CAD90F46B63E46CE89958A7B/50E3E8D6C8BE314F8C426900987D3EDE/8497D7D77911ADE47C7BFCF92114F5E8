using SmartShip.ShipmentService.Domain.Entities;

namespace SmartShip.ShipmentService.Application.DTOs
{
    public class CreateShipmentDto
    {
        public int OriginAddressId { get; set; }

        public int DestinationAddressId { get; set; }

        public ShipmentType ShipmentType { get; set; }

        public string PackageDescription { get; set; } = string.Empty;

        public decimal PackageWeight { get; set; }
    }
}
// why DTOs
//The Shipment entity contains things that the customer shouldn't provide directly.
