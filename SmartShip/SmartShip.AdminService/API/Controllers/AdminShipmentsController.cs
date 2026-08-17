// API/Controllers/AdminShipmentsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShip.AdminService.Application.DTOs;
using SmartShip.AdminService.Application.Interfaces;

namespace SmartShip.AdminService.API.Controllers
{
    [ApiController]
    [Route("api/admin/shipments")]
    [Authorize(Roles = "Admin")]
    public class AdminShipmentsController : ControllerBase
    {
        private readonly IShipmentManagementService _shipmentManagementService;

        public AdminShipmentsController(IShipmentManagementService shipmentManagementService)
        {
            _shipmentManagementService = shipmentManagementService;
        }

        [HttpPut("{shipmentId}/progress")]
        public async Task<IActionResult> UpdateProgress(int shipmentId, [FromBody] UpdateShipmentProgressDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _shipmentManagementService.UpdateShipmentProgressAsync(shipmentId, dto);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}