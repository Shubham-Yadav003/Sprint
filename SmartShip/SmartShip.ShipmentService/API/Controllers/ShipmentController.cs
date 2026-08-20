using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using SmartShip.ShipmentService.Application.DTOs;
using SmartShip.ShipmentService.Application.Interfaces;


namespace SmartShip.ShipmentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   
    public class ShipmentController: ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ShipmentController(IShipmentService shipmentService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _shipmentService = shipmentService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateShipment(CreateShipmentDto dto)
        {
            var customerId = int.Parse( // User represents Security Context of http response
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);


            var shipment = await _shipmentService.CreateShipmentAsync(dto, customerId);

            return Ok(shipment);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Customer")]
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
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAllShipments()
        {
            var customerId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var shipments = await _shipmentService.GetAllShipmentsAsync(customerId);

            return Ok(shipments);
        }

        [HttpPost("{id}/book")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> BookShipment(int id)
        {
            var customerId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _shipmentService.BookShipmentAsync(id, customerId);

            if (!result)
            {
                return NotFound();
            }

            var trackingPayload = JsonSerializer.Serialize(new
            {
                shipmentId = id,
                status = "Booked",
                location = "Booking portal",
                description = "Shipment booked successfully."
            });

            var client = _httpClientFactory.CreateClient();// httpClient instance

            client.DefaultRequestHeaders.Add("X-Service-Key", _configuration["ServiceAuth:Key"]);
            var trackingResponse = await client.PostAsync(
                $"{_configuration["ServiceUrls:TrackingService"]}/api/Tracking/internal",
                new StringContent(trackingPayload, Encoding.UTF8, "application/json")); // {encodig.utf8 -> convert to bytes using -> utf8}
            
            

            if (!trackingResponse.IsSuccessStatusCode)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new
                {
                    message = "Shipment booked, but its tracking event could not be created. Please contact support."
                });
            }

            return Ok(new { message = "Shipment booked successfully." });
        }


        [HttpPost("internal/{id}/status")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateShipmentStatusInternal(int id, UpdateShipmentStatusDto dto)
        {
            if (Request.Headers["X-Service-Key"] != _configuration["ServiceAuth:Key"])
            {
                return Unauthorized();
            }

            var result = await _shipmentService.UpdateShipmentStatusAsync(id, dto.Status);

            if (!result)
            {
                return BadRequest(new {message = "Invalid shipment status transition or shipment not found."});
            }

            return Ok(new { message = "Shipment status updated successfully." });

        }
    }
}
