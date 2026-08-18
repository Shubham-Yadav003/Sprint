using Microsoft.EntityFrameworkCore;
using SmartShip.AdminService.Application.Services;
using SmartShip.AdminService.Domain.Entities;
using SmartShip.AdminService.Infrastructure.Data;
using Xunit;

namespace SmartShip.AdminService.Tests
{
    public class ReportServiceTests
    {
        private AdminDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AdminDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AdminDbContext(options);
        }

        [Fact]
        public async Task GetOperationalSummaryAsync_ShouldCalculateCorrectMetrics()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();

            // Seed locations (2 active, 1 inactive)
            context.Locations.AddRange(
                new Location { Name = "Hub A", Address = "Add 1", City = "Delhi", IsActive = true },
                new Location { Name = "Hub B", Address = "Add 2", City = "Mumbai", IsActive = true },
                new Location { Name = "Hub C", Address = "Add 3", City = "Pune", IsActive = false }
            );

            // Seed delivery issues (2 open, 1 resolved)
            context.DeliveryIssues.AddRange(
                new DeliveryIssue { ShipmentId = 1, IssueType = IssueType.Delayed, Description = "D1", Status = "Open" },
                new DeliveryIssue { ShipmentId = 2, IssueType = IssueType.WeatherDelay, Description = "D2", Status = "Open" },
                new DeliveryIssue { ShipmentId = 3, IssueType = IssueType.Damaged, Description = "D3", Status = "Resolved" }
            );

            await context.SaveChangesAsync();

            var service = new ReportService(context);

            // Act
            var summary = await service.GetOperationalSummaryAsync();

            // Assert
            Assert.NotNull(summary);

            // Location Metrics
            Assert.Equal(3, summary.Locations.Total);
            Assert.Equal(2, summary.Locations.Active);
            Assert.Equal(1, summary.Locations.Inactive);

            // Issue Metrics
            Assert.Equal(3, summary.DeliveryIssues.Total);
            Assert.Equal(2, summary.DeliveryIssues.Open);
            Assert.Equal(1, summary.DeliveryIssues.Resolved);
        }
    }
}