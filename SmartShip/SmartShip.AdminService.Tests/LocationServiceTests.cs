using Microsoft.EntityFrameworkCore;
using SmartShip.AdminService.Application.DTOs;
using SmartShip.AdminService.Application.Services;
using SmartShip.AdminService.Domain.Entities;
using SmartShip.AdminService.Infrastructure.Data;
using Xunit;

namespace SmartShip.AdminService.Tests
{
    public class LocationServiceTests
    {
        private AdminDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AdminDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AdminDbContext(options);
        }

        [Fact]
        public async Task CreateLocationAsync_ShouldAddNewLocation_WithActiveStatusTrue()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new LocationService(context);

            var dto = new CreateLocationDto
            {
                Name = "Jaipur Hub",
                Address = "Plot 12, Sitapura",
                City = "Jaipur"
            };

            // Act
            var result = await service.CreateLocationAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal("Jaipur Hub", result.Name);
            Assert.True(result.IsActive);

            var saved = await context.Locations.FindAsync(result.Id);
            Assert.NotNull(saved);
            Assert.Equal("Jaipur", saved.City);
        }

        [Fact]
        public async Task GetAllLocationsAsync_ShouldReturnAllSavedLocations()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            context.Locations.AddRange(
                new Location { Name = "Delhi Hub", Address = "Phase 1", City = "Delhi", IsActive = true },
                new Location { Name = "Mumbai Hub", Address = "Andheri", City = "Mumbai", IsActive = true }
            );
            await context.SaveChangesAsync();

            var service = new LocationService(context);

            // Act
            var result = await service.GetAllLocationsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetLocationByIdAsync_ShouldReturnNull_WhenLocationDoesNotExist()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new LocationService(context);

            // Act
            var result = await service.GetLocationByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ToggleLocationStatusAsync_ShouldInvertIsActiveStatus()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var location = new Location
            {
                Name = "Noida Hub",
                Address = "Sector 62",
                City = "Noida",
                IsActive = true
            };
            context.Locations.Add(location);
            await context.SaveChangesAsync();

            var service = new LocationService(context);

            // Act - Toggle to false
            var success = await service.ToggleLocationStatusAsync(location.Id);

            // Assert
            Assert.True(success);
            var updated = await context.Locations.FindAsync(location.Id);
            Assert.NotNull(updated);
            Assert.False(updated.IsActive);
        }

        [Fact]
        public async Task ToggleLocationStatusAsync_ShouldReturnFalse_WhenLocationNotFound()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new LocationService(context);

            // Act
            var success = await service.ToggleLocationStatusAsync(999);

            // Assert
            Assert.False(success);
        }
    }
}