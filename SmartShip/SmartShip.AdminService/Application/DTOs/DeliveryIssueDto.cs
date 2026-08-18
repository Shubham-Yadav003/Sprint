using System.ComponentModel.DataAnnotations;
using SmartShip.AdminService.Domain.Entities;

namespace SmartShip.AdminService.Application.DTOs
{
    public class DeliveryIssueDto
    {
        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public IssueType IssueType { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Open";
        public string? ResolutionNotes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    public class CreateDeliveryIssueDto
    {
        [Required]
        public int ShipmentId { get; set; }

        [Required]
        public IssueType IssueType { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }

    public class ResolveIssueDto
    {
        [Required]
        [MaxLength(500)]
        public string ResolutionNotes { get; set; } = string.Empty;
    }
}