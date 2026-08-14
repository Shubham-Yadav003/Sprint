using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShip.TrackingService.Application.Interfaces;

namespace SmartShip.TrackingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DeliveryProofController : ControllerBase
    {
        private readonly IDeliveryProofService _deliveryProofService;

        public DeliveryProofController(
            IDeliveryProofService deliveryProofService)
        {
            _deliveryProofService = deliveryProofService;
        }

        [HttpGet("shipment/{shipmentId}")]
        public async Task<IActionResult> GetDeliveryProofs(int shipmentId)
        {
            var result =
                await _deliveryProofService.GetProofsByShipmentIdAsync(
                    shipmentId);

            return Ok(result);
        }
    }
}