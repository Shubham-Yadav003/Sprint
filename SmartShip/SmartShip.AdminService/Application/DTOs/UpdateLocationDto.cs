using System.ComponentModel.DataAnnotations;

namespace SmartShip.AdminService.Application.DTOs
{
    public class UpdateLocationDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
