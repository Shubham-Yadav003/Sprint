using System.ComponentModel.DataAnnotations;

namespace SmartShip.TrackingService.Domain.Entities
{
    public class TrackingEvent
    {
        [Key]
        public int Id {  get; set; }

        [Range(1, int.MaxValue)]
        public int ShipmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Location {  get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Description {  get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
