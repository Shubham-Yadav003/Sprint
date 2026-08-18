using System.ComponentModel.DataAnnotations;

namespace SmartShip.IdentityService.Application.DTOs // DTO is what data user is allowed to send 
                                                     // we are not using user directly because we don't want to expose the user entity directly to the outside world.
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }= string.Empty;

        [Required]
        [MinLength(6)]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

       
    }
}
