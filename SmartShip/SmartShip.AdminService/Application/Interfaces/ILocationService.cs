using SmartShip.AdminService.Application.DTOs;
namespace SmartShip.AdminService.Application.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationDto>> GetAllLocationsAsync();

        Task<LocationDto?> GetLocationByIdAsync(int id);

        Task<LocationDto> CreateLocationAsync(CreateLocationDto dto);

        Task<bool> UpdateLocationAsync(int id, UpdateLocationDto dto);

        Task<bool> ToggleLocationStatusAsync(int id);
    }
}
