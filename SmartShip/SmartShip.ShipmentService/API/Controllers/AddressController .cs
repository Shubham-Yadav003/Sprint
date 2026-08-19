using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShip.ShipmentService.Application.DTOs;
using SmartShip.ShipmentService.Application.Interfaces;
using System.Security.Claims;

namespace SmartShip.ShipmentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(CreateAddressDto dto)
        {
            var customerId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var address = await _addressService.CreateAddressAsync(
                dto,
                customerId);

            return Ok(address);
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var customerId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var addresses = await _addressService.GetAddressesAsync(customerId);
            return Ok(addresses);
        }
    }
}
