// API/Controllers/AdminIssuesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShip.AdminService.Application.DTOs;
using SmartShip.AdminService.Application.Interfaces;

namespace SmartShip.AdminService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminIssuesController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public AdminIssuesController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        // POST: api/AdminIssues
        [HttpPost]
        public async Task<IActionResult> CreateIssue(CreateDeliveryIssueDto dto)
        {
            var result = await _issueService.CreateIssueAsync(dto);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        // GET: api/AdminIssues?status=Open
        [HttpGet]
        public async Task<IActionResult> GetAllIssues([FromQuery] string? status)
        {
            var issues = await _issueService.GetAllIssuesAsync(status);
            return Ok(issues);
        }

        // PUT: api/AdminIssues/5/resolve
        [HttpPut("{id}/resolve")]
        public async Task<IActionResult> ResolveIssue(int id, ResolveIssueDto dto)
        {
            var success = await _issueService.ResolveIssueAsync(id, dto);

            if (!success)
            {
                return NotFound(new { message = $"Delivery issue with ID {id} was not found." });
            }

            return Ok(new { message = "Issue resolved successfully." });
        }
    }
}