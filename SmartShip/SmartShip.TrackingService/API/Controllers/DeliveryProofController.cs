using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShip.TrackingService.Application.Interfaces;
using SmartShip.TrackingService.Application.DTOs;

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

        [HttpPost("upload")]
        public async Task<ActionResult<DeliveryProofDto>>
           UploadDeliveryProof(
               [FromForm] int shipmentId,
               [FromForm] string proofType,
               IFormFile file)
        {
            var result =
                await _deliveryProofService.UploadDeliveryProofAsync(
                    shipmentId,
                    proofType,
                    file);

            return Ok(result);
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