using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShip.TrackingService.Application.DTOs;
using SmartShip.TrackingService.Application.Interfaces;
namespace SmartShip.TrackingService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TrackingController: ControllerBase
    {
        private readonly ITrackingService _trackingService;

        public TrackingController(ITrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrackingEvent(
            CreateTrackingEventDto dto)
        {
            var result = await _trackingService.CreateTrackingEventAsync(dto);

            return Ok(result);
        }

        [HttpGet("shipment/{shipmentId}")]
        public async Task<IActionResult> GetTrackingEvents(int shipmentId)
        {
            var result = await _trackingService.GetTrackingEventsByShipmentIdAsync(
                    shipmentId);

            return Ok(result);
        }

    }
}
