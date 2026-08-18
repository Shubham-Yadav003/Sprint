using Microsoft.EntityFrameworkCore;
using SmartShip.AdminService.Application.DTOs;
using SmartShip.AdminService.Application.Services;
using SmartShip.AdminService.Domain.Entities;
using SmartShip.AdminService.Infrastructure.Data;
using Xunit;

namespace SmartShip.AdminService.Tests
{
    public class IssueServiceTests
    {
        private AdminDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AdminDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AdminDbContext(options);
        }

        [Fact]
        public async Task CreateIssueAsync_ShouldCreateIssueWithOpenStatus()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new IssueService(context);

            var dto = new CreateDeliveryIssueDto
            {
                ShipmentId = 10,
                IssueType = IssueType.AddressNotFound,
                Description = "Recipient address landmark not found."
            };

            // Act
            var result = await service.CreateIssueAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal("Open", result.Status);
            Assert.Equal(IssueType.AddressNotFound, result.IssueType);

            var saved = await context.DeliveryIssues.FindAsync(result.Id);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task GetAllIssuesAsync_ShouldFilterByStatus_WhenStatusProvided()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            context.DeliveryIssues.AddRange(
                new DeliveryIssue { ShipmentId = 1, IssueType = IssueType.Delayed, Description = "Fog", Status = "Open" },
                new DeliveryIssue { ShipmentId = 2, IssueType = IssueType.Damaged, Description = "Box torn", Status = "Resolved" }
            );
            await context.SaveChangesAsync();

            var service = new IssueService(context);

            // Act
            var openIssues = await service.GetAllIssuesAsync("Open");

            // Assert
            Assert.Single(openIssues);
            Assert.Equal("Open", openIssues.First().Status);
        }

        [Fact]
        public async Task ResolveIssueAsync_ShouldUpdateStatusToResolved_AndSetResolvedAt()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var issue = new DeliveryIssue
            {
                ShipmentId = 5,
                IssueType = IssueType.CustomerUnavailable,
                Description = "Customer phone unreachable",
                Status = "Open"
            };
            context.DeliveryIssues.Add(issue);
            await context.SaveChangesAsync();

            var service = new IssueService(context);
            var resolveDto = new ResolveIssueDto
            {
                ResolutionNotes = "Contacted alternate number, delivery rescheduled."
            };

            // Act
            var success = await service.ResolveIssueAsync(issue.Id, resolveDto);

            // Assert
            Assert.True(success);
            var updated = await context.DeliveryIssues.FindAsync(issue.Id);
            Assert.NotNull(updated);
            Assert.Equal("Resolved", updated.Status);
            Assert.NotNull(updated.ResolvedAt);
            Assert.Equal(resolveDto.ResolutionNotes, updated.ResolutionNotes);
        }

        [Fact]
        public async Task ResolveIssueAsync_ShouldReturnFalse_WhenIssueNotFound()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new IssueService(context);
            var resolveDto = new ResolveIssueDto { ResolutionNotes = "No note" };

            // Act
            var success = await service.ResolveIssueAsync(999, resolveDto);

            // Assert
            Assert.False(success);
        }
    }
}