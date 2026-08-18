using Microsoft.EntityFrameworkCore;
using SmartShip.AdminService.Application.DTOs;
using SmartShip.AdminService.Application.Interfaces;
using SmartShip.AdminService.Infrastructure.Data;

namespace SmartShip.AdminService.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly AdminDbContext _context;

        public ReportService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<OperationalSummaryDto> GetOperationalSummaryAsync()
        {
            var totalLocations = await _context.Locations.CountAsync();
            var activeLocations = await _context.Locations.CountAsync(l => l.IsActive);

            var totalIssues = await _context.DeliveryIssues.CountAsync();
            var openIssues = await _context.DeliveryIssues.CountAsync(i => i.Status == "Open");
            var resolvedIssues = await _context.DeliveryIssues.CountAsync(i => i.Status == "Resolved");

            return new OperationalSummaryDto
            {
                Locations = new LocationMetricsDto
                {
                    Total = totalLocations,
                    Active = activeLocations,
                    Inactive = totalLocations - activeLocations
                },
                DeliveryIssues = new IssueMetricsDto
                {
                    Total = totalIssues,
                    Open = openIssues,
                    Resolved = resolvedIssues
                },
                GeneratedAt = DateTime.UtcNow
            };
        }
    }
}