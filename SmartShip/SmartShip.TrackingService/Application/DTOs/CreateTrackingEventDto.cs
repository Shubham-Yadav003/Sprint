using System.ComponentModel.DataAnnotations;

namespace SmartShip.TrackingService.Application.DTOs
{
    public class CreateTrackingEventDto
    {
        [Range(1, int.MaxValue)]
        public int ShipmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Description { get; set; } = string.Empty;
    }
}
