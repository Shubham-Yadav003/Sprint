using SmartShip.ShipmentService.Application.DTOs;
using SmartShip.ShipmentService.Domain.Entities;

namespace SmartShip.ShipmentService.Application.Interfaces
{
    public interface IAddressService
    {
        Task<Address> CreateAddressAsync(
            CreateAddressDto dto,
            int customerId);

        Task<List<Address>> GetAddressesAsync(int customerId);
    }
}
