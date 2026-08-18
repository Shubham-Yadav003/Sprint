using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShip.AdminService.Application.Interfaces;

namespace SmartShip.AdminService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public AdminReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: api/AdminReports/summary
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var report = await _reportService.GetOperationalSummaryAsync();
            return Ok(report);
        }
    }
}