using System.ComponentModel.DataAnnotations;

namespace SmartShip.ShipmentService.Domain.Entities
{
    public class Shipment
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

        [Range(1, int.MaxValue)]
        public int OriginAddressId { get; set; }

        [Range(1, int.MaxValue)]
        public int DestinationAddressId { get; set; }

        public ShipmentType ShipmentType { get; set; }

        public ShipmentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}