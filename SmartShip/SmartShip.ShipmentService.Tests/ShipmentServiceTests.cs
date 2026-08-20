using Microsoft.EntityFrameworkCore;
using SmartShip.ShipmentService.Application.DTOs;
using SmartShip.ShipmentService.Domain.Entities;
using SmartShip.ShipmentService.Infrastructure.Data;
using ShipmentServiceImpl = SmartShip.ShipmentService.Application.Services.ShipmentService;

namespace SmartShip.ShipmentService.Tests
{
    public class ShipmentServiceTests
    {
        [Fact]
        public async Task CreateShipmentAsync_WithValidData_CreatesShipment() // create a shipment with valid data->convention{Methodname, condition,ExpectedResult}
        {
            var options = new DbContextOptionsBuilder<ShipmentDbContext>() // instructd ef core  to use inmemory instead of sqlserver
                .UseInMemoryDatabase("CreateShipmentTest")
                .Options;

            using var context = new ShipmentDbContext(options); // when test finishes, context will be automatically siposed of , {using -> prevents memory leaks}

            context.Addresses.AddRange(
                new Address
                {
                    Id = 1,
                    CustomerId = 1,
                    Addressline = "25 Civil Lines",
                    City = "Prayagraj",
                    State = "Uttar Pradesh",
                    PostalCode = "211001"
                },
                new Address
                {
                    Id = 2,
                    CustomerId = 1,
                    Addressline = "10 MG Road",
                    City = "Lucknow",
                    State = "Uttar Pradesh",
                    PostalCode = "226001"
                });

            await context.SaveChangesAsync();

            var service = new ShipmentServiceImpl(context);

            var dto = new CreateShipmentDto
            {
                OriginAddressId = 1,
                DestinationAddressId = 2,
                ShipmentType = ShipmentType.Express,
                PackageDescription = "Laptop",
                PackageWeight = 2
            };

            var result = await service.CreateShipmentAsync(dto, 1);

            Assert.NotNull(result);
            Assert.Equal(1, result.CustomerId);
            Assert.Equal(1, result.OriginAddressId);
            Assert.Equal(2, result.DestinationAddressId);
            Assert.Equal(ShipmentType.Express, result.ShipmentType);
            Assert.Equal(ShipmentStatus.Draft, result.Status);
        }

        // reject invalid address id
        [Fact]
        public async Task CreateShipmentAsync_WithInvalidOriginAddress_ThrowsException()
        {
            var options = new DbContextOptionsBuilder<ShipmentDbContext>()
                .UseInMemoryDatabase("InvalidOriginTest")
                .Options;

            using var context = new ShipmentDbContext(options);

            context.Addresses.Add(new Address
            {
                Id = 2,
                CustomerId = 1,
                Addressline = "10 MG Road",
                City = "Lucknow",
                State = "Uttar Pradesh",
                PostalCode = "226001"
            });

            await context.SaveChangesAsync();

            var service = new ShipmentServiceImpl(context);

            var dto = new CreateShipmentDto
            {
                OriginAddressId = 999,
                DestinationAddressId = 2,
                ShipmentType = ShipmentType.Express,
                PackageDescription = "Laptop",
                PackageWeight = 2
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CreateShipmentAsync(dto, 1));
        }

        // Book Shipment
        [Fact]
        public async Task BookShipmentAsync_WithDraftShipment_BooksShipment()
        {
            var options = new DbContextOptionsBuilder<ShipmentDbContext>()
                .UseInMemoryDatabase("BookShipmentTest")
                .Options;

            using var context = new ShipmentDbContext(options);

            var shipment = new Shipment
            {
                Id = 1,
                CustomerId = 1,
                OriginAddressId = 1,
                DestinationAddressId = 2,
                ShipmentType = ShipmentType.Express,
                Status = ShipmentStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            context.Shipments.Add(shipment);
            await context.SaveChangesAsync();

            var service = new ShipmentServiceImpl(context);

            var result = await service.BookShipmentAsync(1, 1);

            Assert.True(result);
            Assert.Equal(ShipmentStatus.Booked, shipment.Status);
        }

        // reject for Booked → Delivered 
        [Fact]
        public async Task UpdateShipmentStatusAsync_InvalidTransition_ReturnsFalse()
        {
            var options = new DbContextOptionsBuilder<ShipmentDbContext>()
                .UseInMemoryDatabase("InvalidStatusTest")
                .Options;

            using var context = new ShipmentDbContext(options);

            var shipment = new Shipment
            {
                Id = 1,
                CustomerId = 1,
                OriginAddressId = 1,
                DestinationAddressId = 2,
                ShipmentType = ShipmentType.Express,
                Status = ShipmentStatus.Booked,
                CreatedAt = DateTime.UtcNow
            };

            context.Shipments.Add(shipment);
            await context.SaveChangesAsync();

            var service = new ShipmentServiceImpl(context);

            var result = await service.UpdateShipmentStatusAsync(
                1,
                ShipmentStatus.Delivered);

            Assert.False(result);
            Assert.Equal(ShipmentStatus.Booked, shipment.Status);
        }

        // a valid status transition.
        [Fact]
        public async Task UpdateShipmentStatusAsync_ValidTransition_UpdatesStatus()
        {
            var options = new DbContextOptionsBuilder<ShipmentDbContext>()
                .UseInMemoryDatabase("ValidStatusTest")
                .Options;

            using var context = new ShipmentDbContext(options);

            var shipment = new Shipment
            {
                Id = 1,
                CustomerId = 1,
                OriginAddressId = 1,
                DestinationAddressId = 2,
                ShipmentType = ShipmentType.Express,
                Status = ShipmentStatus.Booked,
                CreatedAt = DateTime.UtcNow
            };

            context.Shipments.Add(shipment);
            await context.SaveChangesAsync();

            var service = new ShipmentServiceImpl(context);

            var result = await service.UpdateShipmentStatusAsync(
                1,
                ShipmentStatus.PickedUp);

            Assert.True(result);
            Assert.Equal(ShipmentStatus.PickedUp, shipment.Status);
        }
    }
}
