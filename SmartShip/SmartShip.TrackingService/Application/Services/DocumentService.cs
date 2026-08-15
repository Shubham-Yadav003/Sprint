using Microsoft.EntityFrameworkCore;
using SmartShip.TrackingService.Application.DTOs;
using SmartShip.TrackingService.Application.Interfaces;
using SmartShip.TrackingService.Domain.Entities;
using SmartShip.TrackingService.Infrastructure.Data;

namespace SmartShip.TrackingService.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly TrackingDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public DocumentService(TrackingDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<ShipmentDocumentDto> UploadDocumentAsync(
             int shipmentId,
            string documentType,
            IFormFile file)
        {

            if (shipmentId <= 0)
            {
                throw new ArgumentException("Invalid shipment ID.");
            }

            if (string.IsNullOrWhiteSpace(documentType))
            {
                throw new ArgumentException("Document type is required.");
            }

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required.");
            }

            var filePath = await _fileStorageService.SaveFileAsync(file, "Documents");
            var document = new ShipmentDocument
            {
                ShipmentId = shipmentId,
                DocumentType = documentType,
                FileName = file.FileName,
                FilePath = filePath,
                UploadedAt = DateTime.UtcNow
            };

            _context.ShipmentDocuments.Add(document);

            await _context.SaveChangesAsync();

            return new ShipmentDocumentDto // return dto
            {
                Id = document.Id,
                ShipmentId = document.ShipmentId,
                DocumentType = document.DocumentType,
                FileName = file.FileName,
                FilePath = filePath,
                UploadedAt = document.UploadedAt
            };
        }

        public async Task<List<ShipmentDocumentDto>> GetDocumentsByShipmentIdAsync(
            int shipmentId)
        {
            return await _context.ShipmentDocuments
                .Where(x => x.ShipmentId == shipmentId)
                .OrderByDescending(x => x.UploadedAt)
                .Select(x => new ShipmentDocumentDto
                {
                    Id = x.Id,
                    ShipmentId = x.ShipmentId,
                    DocumentType = x.DocumentType,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    UploadedAt = x.UploadedAt
                })
                .ToListAsync();
        }
    }
}