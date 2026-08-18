using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using SmartShip.AdminService.Application.DTOs;
using SmartShip.AdminService.Application.Services;
using SmartShip.AdminService.Domain.Entities;
using SmartShip.AdminService.Infrastructure.Data;
using Xunit;

namespace SmartShip.AdminService.Tests
{
    public class ShipmentManagementServiceTests
    {
        private AdminDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AdminDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AdminDbContext(options);
        }

        private IConfiguration CreateMockConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "ServiceUrls:ShipmentService", "https://localhost:7116" },
                { "ServiceUrls:TrackingService", "https://localhost:7107" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public async Task UpdateShipmentProgressAsync_ShouldReturnFailure_WhenLocationDoesNotExist()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var config = CreateMockConfiguration();
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();

            var service = new ShipmentManagementService(context, mockFactory.Object, config, mockContextAccessor.Object);

            var dto = new UpdateShipmentProgressDto
            {
                LocationId = 999, // Non-existent
                Status = "InTransit",
                Description = "Arrived at hub"
            };

            // Act
            var (success, message) = await service.UpdateShipmentProgressAsync(1, dto);

            // Assert
            Assert.False(success);
            Assert.Contains("does not exist", message);
        }

        [Fact]
        public async Task UpdateShipmentProgressAsync_ShouldReturnFailure_WhenLocationIsInactive()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var location = new Location { Name = "Closed Hub", Address = "Add", City = "City", IsActive = false };
            context.Locations.Add(location);
            await context.SaveChangesAsync();

            var config = CreateMockConfiguration();
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();

            var service = new ShipmentManagementService(context, mockFactory.Object, config, mockContextAccessor.Object);

            var dto = new UpdateShipmentProgressDto
            {
                LocationId = location.Id,
                Status = "InTransit",
                Description = "Arrived at hub"
            };

            // Act
            var (success, message) = await service.UpdateShipmentProgressAsync(1, dto);

            // Assert
            Assert.False(success);
            Assert.Contains("inactive", message);
        }

        [Fact]
        public async Task UpdateShipmentProgressAsync_ShouldSucceed_WhenServicesReturn200()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var location = new Location { Name = "Active Hub", Address = "Add", City = "City", IsActive = true };
            context.Locations.Add(location);
            await context.SaveChangesAsync();

            var config = CreateMockConfiguration();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();

            // Mock HttpMessageHandler returning 200 OK for both calls
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = new HttpClient(mockHandler.Object);
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var service = new ShipmentManagementService(context, mockFactory.Object, config, mockContextAccessor.Object);

            var dto = new UpdateShipmentProgressDto
            {
                LocationId = location.Id,
                Status = "InTransit",
                Description = "Arrived at active hub"
            };

            // Act
            var (success, message) = await service.UpdateShipmentProgressAsync(1, dto);

            // Assert
            Assert.True(success);
            Assert.Contains("Active Hub", message);
        }
    }
}