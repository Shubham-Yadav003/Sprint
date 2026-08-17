using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShip.TrackingService.Application.DTOs;
using SmartShip.TrackingService.Application.Interfaces;
namespace SmartShip.TrackingService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class TrackingController: ControllerBase
    {
        private readonly ITrackingService _trackingService;

        public TrackingController(ITrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTrackingEvent(
            CreateTrackingEventDto dto)
        {
            var result = await _trackingService.CreateTrackingEventAsync(dto);

            return Ok(result);
        }

        [HttpGet("shipment/{shipmentId}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> GetTrackingEvents(int shipmentId)
        {
            var result = await _trackingService.GetTrackingEventsByShipmentIdAsync(
                    shipmentId);

            return Ok(result);
        }

    }
}
