// xUnit , For writing tests:
// Moq, for mocking
// EF Core InMemory, Instead of connecting our tests to your actual SQL Server:


using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using SmartShip.TrackingService.Application.Interfaces;
using SmartShip.TrackingService.Application.Services;
using SmartShip.TrackingService.Infrastructure.Data;
using DocumentServiceImpl =  SmartShip.TrackingService.Application.Services.DocumentService;
namespace SmartShip.TrackingService.Tests
{
    public class DocumentServiceTests
    {
        private TrackingDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TrackingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TrackingDbContext(options);
        }

        [Fact]
        public async Task UploadDocument_ShouldSaveDocument()
        {
            // Arrange
            var context = GetDbContext();

            var fileStorageMock = new Mock<IFileStorageService>();

            fileStorageMock  // send this file location whenever saveFile  is called
                .Setup(x => x.SaveFileAsync(
                    It.IsAny<IFormFile>(),
                    "Documents"))
                .ReturnsAsync("UploadedFiles/Documents/test-file.pdf");

            var service = new DocumentService(
                context,
                fileStorageMock.Object);

            var fileContent = new MemoryStream(
                System.Text.Encoding.UTF8.GetBytes("Test file content"));

            var file = new FormFile( // it will behave like an uploded file
                fileContent,
                0,
                fileContent.Length,
                "file",
                "test-file.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            // Act
            var result = await service.UploadDocumentAsync(
                25,
                "Invoice",
                file);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(25, result.ShipmentId);
            Assert.Equal("Invoice", result.DocumentType);
            Assert.Equal("test-file.pdf", result.FileName);
            Assert.Equal(
                "UploadedFiles/Documents/test-file.pdf",
                result.FilePath);

            var savedDocument = await context.ShipmentDocuments
                .FirstOrDefaultAsync();

            Assert.NotNull(savedDocument);
            Assert.Equal(25, savedDocument.ShipmentId);
            Assert.Equal("Invoice", savedDocument.DocumentType);
            Assert.Equal("test-file.pdf", savedDocument.FileName);

            fileStorageMock.Verify(
                x => x.SaveFileAsync(
                    It.IsAny<IFormFile>(),
                    "Documents"),
                Times.Once); // Did DocumentService actually call FileStorageService, time.once means exactly once
        }


        [Fact]
        public async Task UploadDocument_ShouldRejectInvalidShipmentId()
        {
            // Arrange
            var context = GetDbContext();

            var fileStorageMock = new Mock<IFileStorageService>();

            var service = new DocumentService(
                context,
                fileStorageMock.Object);

            var file = new Mock<IFormFile>().Object;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.UploadDocumentAsync(
                    0,
                    "Invoice",
                    file));
        } // no need of mock because it will fail it shipmentId<0 check
    }
}
