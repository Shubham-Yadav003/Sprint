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

        // Optional for normal progress updates. Provide this for issue types such
        // as Damaged, AddressNotFound, or WeatherDelay.
        public global::SmartShip.AdminService.Domain.Entities.IssueType? IssueType { get; set; }
    }
}

