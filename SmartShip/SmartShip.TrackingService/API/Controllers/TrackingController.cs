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
        private readonly IConfiguration _configuration;

        public TrackingController(ITrackingService trackingService, IConfiguration configuration)
        {
            _trackingService = trackingService;
            _configuration = configuration;
        }

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> CreateTrackingEvent(
        //    CreateTrackingEventDto dto)
        //{
        //    var result = await _trackingService.CreateTrackingEventAsync(dto);

        //    return Ok(result);
        //}

        [HttpPost("internal")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateInternalTrackingEvent(CreateTrackingEventDto dto)
        {
            if (Request.Headers["X-Service-Key"] != _configuration["ServiceAuth:Key"])
            {
                return Unauthorized();
            }

            var result = await _trackingService.CreateTrackingEventAsync(dto);
            return Ok(result);
        }

        [HttpGet("shipment/{shipmentId}")] // tracking event trailer
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> GetTrackingEvents(int shipmentId)
        {
            var result = await _trackingService.GetTrackingEventsByShipmentIdAsync(
                    shipmentId);

            return Ok(result);
        }

    }
}
