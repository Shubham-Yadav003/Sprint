using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SmartShip.AdminService.Application.DTOs;
using SmartShip.AdminService.Application.Interfaces;
using SmartShip.AdminService.Infrastructure.Data;

namespace SmartShip.AdminService.Application.Services
{
    public class ShipmentManagementService : IShipmentManagementService
    {
        private readonly AdminDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShipmentManagementService(
            AdminDbContext context,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool Success, string Message)> UpdateShipmentProgressAsync(int shipmentId, UpdateShipmentProgressDto dto)
        {
            // 1. Fetch location from Admin DB to resolve the name
            var location = await _context.Locations.FindAsync(dto.LocationId);
            if (location == null)
            {
                return (false, $"Location with ID {dto.LocationId} does not exist.");
            }

            if (!location.IsActive)
            {
                return (false, $"Location '{location.Name}' is currently inactive.");
            }

            // 2. Prepare HTTP client for internal service calls.
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Service-Key", _configuration["ServiceAuth:Key"]);

            var shipmentBaseUrl = _configuration["ServiceUrls:ShipmentService"];
            var trackingBaseUrl = _configuration["ServiceUrls:TrackingService"];

            // 3. Update status in ShipmentService (POST /api/Shipment/{id}/status)
            var statusPayload = JsonSerializer.Serialize(new { status = dto.Status });
            var statusContent = new StringContent(statusPayload, Encoding.UTF8, "application/json");

            var shipmentResponse = await client.PostAsync($"{shipmentBaseUrl}/api/Shipment/internal/{shipmentId}/status", statusContent);
            if (!shipmentResponse.IsSuccessStatusCode)
            {
                return (false, $"Failed to update status in ShipmentService. (Status: {shipmentResponse.StatusCode})");
            }

            // 4. Create tracking event in TrackingService (POST /api/Tracking)
            var trackingPayload = JsonSerializer.Serialize(new
            {
                shipmentId = shipmentId,
                status = dto.Status,
                location = location.Name,
                description = dto.Description
            });
            var trackingContent = new StringContent(trackingPayload, Encoding.UTF8, "application/json");

            var trackingResponse = await client.PostAsync($"{trackingBaseUrl}/api/Tracking/internal", trackingContent);
            if (!trackingResponse.IsSuccessStatusCode)
            {
                return (false, $"Shipment status updated, but failed to log tracking event. (Status: {trackingResponse.StatusCode})");
            }

            // A Delayed status is always a delivery issue. For other issue types,
            // the admin supplies dto.IssueType in the progress request.
            var issueType = dto.IssueType;
            if (issueType == null && string.Equals(dto.Status, "Delayed", StringComparison.OrdinalIgnoreCase))
            {
                issueType = global::SmartShip.AdminService.Domain.Entities.IssueType.Delayed;
            }

            if (issueType != null)
            {
                _context.DeliveryIssues.Add(new global::SmartShip.AdminService.Domain.Entities.DeliveryIssue
                {
                    ShipmentId = shipmentId,
                    IssueType = issueType.Value,
                    Description = dto.Description,
                    Status = "Open",
                    CreatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }

            var issueMessage = issueType != null ? " An open delivery issue was created." : string.Empty;
            return (true, $"Shipment status updated to '{dto.Status}' and tracking event logged at '{location.Name}'.{issueMessage}");
        }
    }
}
