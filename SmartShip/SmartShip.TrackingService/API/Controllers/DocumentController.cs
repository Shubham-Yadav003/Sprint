using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShip.TrackingService.Application.DTOs;
using SmartShip.TrackingService.Application.Interfaces;
namespace SmartShip.TrackingService.API.Controllers
{
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
