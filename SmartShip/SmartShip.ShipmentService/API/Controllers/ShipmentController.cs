using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SmartShip.ShipmentService.Application.DTOs;
using SmartShip.ShipmentService.Application.Interfaces;


namespace SmartShip.ShipmentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class ShipmentController: ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipment(CreateShipmentDto dto)
        {
            var customerId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);


            var shipment = await _shipmentService.CreateShipmentAsync(dto, customerId);

            return Ok(shipment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipmentById(int id)
        {
            var customerId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!); // inside jwt in nameidentifier ID is present , ! means it will never be null so dont't warn {null-forgiving operator}

            var shipment = await _shipmentService.GetShipmentByIdAsync(id, customerId);

            if (shipment == null)
            {
                return NotFound();
            }

            return Ok(shipment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShipments()
        {
            var customerId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var shipments = await _shipmentService.GetAllShipmentsAsync(customerId);

            return Ok(shipments);
        }

        [HttpPost("{id}/book")]
        public async Task<IActionResult> BookShipment(int id)
        {
            var customerId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _shipmentService.BookShipmentAsync(id, customerId);

            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Shipment booked successfully." });
        }


        [HttpPost("{id}/status")]
        public async Task<IActionResult> UpdateShipmentStatus(int id, UpdateShipmentStatusDto dto) // dto becz admin can add only certain type of status
        {
            var result = await _shipmentService.UpdateShipmentStatusAsync(id, dto.Status);

            if (!result)
            {
                return BadRequest(new {message = "Invalid shipment status transition or shipment not found."});
            }

            return Ok(new { message = "Shipment status updated successfully." });

        }
    }
}
