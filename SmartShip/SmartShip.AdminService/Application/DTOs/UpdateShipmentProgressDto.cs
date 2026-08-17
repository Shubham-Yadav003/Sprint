using System.ComponentModel.DataAnnotations;
namespace SmartShip.AdminService.Application.DTOs
{
    public class UpdateShipmentProgressDto
    {
        [Required]
        public string Status { get; set; } = string.Empty; // e.g., "InTransit", "OutForDelivery"

        [Required]
        public int LocationId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty; // e.g., "Shipment arrived at Jaipur Hub"
    }
}

