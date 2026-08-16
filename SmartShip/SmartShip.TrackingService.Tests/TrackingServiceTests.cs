using Microsoft.EntityFrameworkCore;
using SmartShip.TrackingService.Application.DTOs;
using SmartShip.TrackingService.Application.Services;
using SmartShip.TrackingService.Domain.Entities;
using SmartShip.TrackingService.Infrastructure.Data;
using TrackingServiceImpl = SmartShip.TrackingService.Application.Services.TrackingService;

//// xUnit , For writing tests:
// Moq, for mocking
// EF Core InMemory, Instead of connecting our tests to your actual SQL Server:

namespace SmartShip.TrackingService.Tests
{
    public class TrackingServiceTests
    {
        // Create the InMemory database
        private TrackingDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TrackingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TrackingDbContext(options); 
        }
        // guid becz every test gets a new database
        [Fact]
        public async Task CreateTrackingEvent_ShouldCreateEvent()
        {
            // Arrange 
            var context = GetDbContext();
            var service = new TrackingServiceImpl(context);

            var dto = new CreateTrackingEventDto
            {
                ShipmentId = 25,
                Status = "In Transit",
                Location = "Delhi Hub",
                Description = "Shipment reached Delhi Hub"
            };

            // Act
            var result = await service.CreateTrackingEventAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(25, result.ShipmentId);
            Assert.Equal("In Transit", result.Status);
            Assert.Equal("Delhi Hub", result.Location);
            Assert.Equal("Shipment reached Delhi Hub", result.Description);

            var savedEvent = await context.TrackingEvents.FirstOrDefaultAsync();

            Assert.NotNull(savedEvent);
            Assert.Equal(25, savedEvent.ShipmentId);



        }

        [Fact]
        public async Task GetTrackingEventsByShipmentId_ShouldReturnEvents()
        {
            // Arrange
            var context = GetDbContext();

            context.TrackingEvents.AddRange(
                new TrackingEvent
                {
                    ShipmentId = 25,
                    Status = "Picked Up",
                    Location = "Delhi",
                    Description = "Shipment picked up",
                    CreatedAt = DateTime.UtcNow.AddHours(-2)
                },
                new TrackingEvent
                {
                    ShipmentId = 25,
                    Status = "In Transit",
                    Location = "Jaipur",
                    Description = "Shipment is in transit",
                    CreatedAt = DateTime.UtcNow.AddHours(-1)
                },
                new TrackingEvent
                {
                    ShipmentId = 30,
                    Status = "Picked Up",
                    Location = "Mumbai",
                    Description = "Shipment picked up",
                    CreatedAt = DateTime.UtcNow
                });

            await context.SaveChangesAsync();

            var service = new TrackingServiceImpl(context);

            // Act
            var result = await service.GetTrackingEventsByShipmentIdAsync(25);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal("Picked Up", result[0].Status);
            Assert.Equal("In Transit", result[1].Status);

            Assert.All(result, x => Assert.Equal(25, x.ShipmentId));
        }
    }
}
