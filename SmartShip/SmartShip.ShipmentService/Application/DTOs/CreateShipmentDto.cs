using System.ComponentModel.DataAnnotations;
using SmartShip.ShipmentService.Domain.Entities;

namespace SmartShip.ShipmentService.Application.DTOs
{
    public class CreateShipmentDto
    {
        [Range(1, int.MaxValue)]
        public int OriginAddressId { get; set; }

        [Range(1, int.MaxValue)]
        public int DestinationAddressId { get; set; }

        public ShipmentType ShipmentType { get; set; }

        [Required]
        [StringLength(250)]
        public string PackageDescription { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "1000000")]
        public decimal PackageWeight { get; set; }
    }
}
// why DTOs
//The Shipment entity contains things that the customer shouldn't provide directly.
