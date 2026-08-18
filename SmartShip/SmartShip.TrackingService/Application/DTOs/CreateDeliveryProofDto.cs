using System.ComponentModel.DataAnnotations;

namespace SmartShip.TrackingService.Application.DTOs
{
    public class CreateDeliveryProofDto
    {
        [Range(1, int.MaxValue)]
        public int ShipmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProofType { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;
    }
}
