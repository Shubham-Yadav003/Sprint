using SmartShip.IdentityService.Application.DTOs; //  response given by user
namespace SmartShip.IdentityService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
        // Task be used becz is an asynchronous method. Instead of freezing or blocking the thread while waiting for a network or database call to complete, it yields control back to the runtime until the operation finishes.

        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
