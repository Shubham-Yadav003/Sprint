using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using SmartShip.TrackingService.Application.Interfaces;
using SmartShip.TrackingService.Application.Services;
using SmartShip.TrackingService.Domain.Entities;
using SmartShip.TrackingService.Infrastructure.Data;
using DeliveryProofServiceImpl = SmartShip.TrackingService.Application.Services.DeliveryProofService;

namespace SmartShip.TrackingService.Tests
{
    public class DeliveryProofServiceTests
    {
        private TrackingDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TrackingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TrackingDbContext(options);
        }

        [Fact]
        public async Task UploadDeliveryProof_ShouldSaveProof()
        {
            var context = GetDbContext();
            var fileStorageMock = new Mock<IFileStorageService>();

            fileStorageMock
                .Setup(x => x.SaveFileAsync(It.IsAny<IFormFile>(), "DeliveryProofs"))
                .ReturnsAsync("UploadedFiles/DeliveryProofs/test-proof.jpeg");

            var service = new DeliveryProofServiceImpl(context, fileStorageMock.Object);

            var fileContent = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("proof"));
            var file = new FormFile(fileContent, 0, fileContent.Length, "file", "proof.jpeg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var result = await service.UploadDeliveryProofAsync(25, "Photo", file);

            Assert.NotNull(result);
            Assert.Equal(25, result.ShipmentId);
            Assert.Equal("Photo", result.ProofType);
            Assert.Equal("proof.jpeg", result.FileName);
            Assert.Equal("UploadedFiles/DeliveryProofs/test-proof.jpeg", result.FilePath);

            var savedProof = await context.DeliveryProofs.FirstOrDefaultAsync();

            Assert.NotNull(savedProof);
            Assert.Equal(25, savedProof.ShipmentId);
            Assert.Equal("Photo", savedProof.ProofType);

            fileStorageMock.Verify(
                x => x.SaveFileAsync(It.IsAny<IFormFile>(), "DeliveryProofs"),
                Times.Once);
        }

        [Fact]
        public async Task UploadDeliveryProof_ShouldRejectEmptyProofType()
        {
            var context = GetDbContext();
            var fileStorageMock = new Mock<IFileStorageService>();
            var service = new DeliveryProofServiceImpl(context, fileStorageMock.Object);

            var file = new Mock<IFormFile>().Object;

            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.UploadDeliveryProofAsync(1, "", file));
        }

        [Fact]
        public async Task GetProofsByShipmentId()
        {
            var context = GetDbContext();

            context.DeliveryProofs.AddRange(
                new DeliveryProof
                {
                    ShipmentId = 10,
                    ProofType = "Photo",
                    FileName = "first.jpeg",
                    FilePath = "UploadedFiles/DeliveryProofs/first.jpeg",
                    UploadedAt = DateTime.UtcNow.AddHours(-2)
                },
                new DeliveryProof
                {
                    ShipmentId = 10,
                    ProofType = "Signature",
                    FileName = "second.jpeg",
                    FilePath = "UploadedFiles/DeliveryProofs/second.jpeg",
                    UploadedAt = DateTime.UtcNow.AddHours(-1)
                },
                new DeliveryProof
                {
                    ShipmentId = 99,
                    ProofType = "Photo",
                    FileName = "other.jpeg",
                    FilePath = "UploadedFiles/DeliveryProofs/other.jpeg",
                    UploadedAt = DateTime.UtcNow
                });

            await context.SaveChangesAsync();

            var fileStorageMock = new Mock<IFileStorageService>();
            var service = new DeliveryProofServiceImpl(context, fileStorageMock.Object);

            var result = await service.GetProofsByShipmentIdAsync(10);

            Assert.Equal(2, result.Count);
            Assert.All(result, x => Assert.Equal(10, x.ShipmentId));
            Assert.Equal("second.jpeg", result[0].FileName);
            Assert.Equal("first.jpeg", result[1].FileName);
        }
    }
}
