using System.ComponentModel.DataAnnotations;

namespace SmartShip.TrackingService.Domain.Entities
{
    public class ShipmentDocument
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int ShipmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string DocumentType { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }= string.Empty;

        public DateTime UploadedAt { get; set; }
    }
}
