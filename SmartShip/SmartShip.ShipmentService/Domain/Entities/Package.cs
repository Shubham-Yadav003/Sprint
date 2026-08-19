using System.ComponentModel.DataAnnotations;

namespace SmartShip.ShipmentService.Domain.Entities
{
    public class Package
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int ShipmentId { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "1000000")]
        public decimal Weight { get; set; }
    }
}
