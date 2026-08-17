using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShip.TrackingService.Application.DTOs;
using SmartShip.TrackingService.Application.Interfaces;
namespace SmartShip.TrackingService.API.Controllers
{//start
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentController: ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<ShipmentDocumentDto>> UploadDocument(
            [FromForm] int shipmentId,
            [FromForm] string documentType,
            IFormFile file)
        {
            var result = await _documentService.UploadDocumentAsync(
               shipmentId,
               documentType,
               file);

            return Ok(result);
        }

        [HttpGet("shipment/{shipmentId}")]
        public async Task<IActionResult> GetDocuments(int shipmentId)
        {
            var result =
                await _documentService.GetDocumentsByShipmentIdAsync(
                    shipmentId);

            return Ok(result);
        }
    }
}
