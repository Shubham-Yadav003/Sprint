using System.ComponentModel.DataAnnotations;

namespace SmartShip.AdminService.Application.DTOs
{
    public class CreateLocationDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;
    }
}
