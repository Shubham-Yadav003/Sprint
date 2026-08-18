using System.ComponentModel.DataAnnotations;

namespace SmartShip.AdminService.Domain.Entities
{
    public class DeliveryIssue
    {
        public int Id { get; set; }

        [Required]
        public int ShipmentId { get; set; }

        [Required]
        public IssueType IssueType { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Open"; // Open, Resolved

        [MaxLength(500)]
        public string? ResolutionNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ResolvedAt { get; set; }
    }
}