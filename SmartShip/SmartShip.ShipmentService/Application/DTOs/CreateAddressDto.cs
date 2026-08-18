using System.ComponentModel.DataAnnotations;

namespace SmartShip.ShipmentService.Application.DTOs
{
    public class CreateAddressDto
    {
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
