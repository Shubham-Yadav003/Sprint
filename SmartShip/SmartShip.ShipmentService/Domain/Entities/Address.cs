using System.ComponentModel.DataAnnotations;

namespace SmartShip.ShipmentService.Domain.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(200)]
        public string Addressline { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string State { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; } = string.Empty;
    }
}