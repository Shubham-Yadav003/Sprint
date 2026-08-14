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

        public DocumentService(TrackingDbContext context)
        {
            _context = context;
        }

        public async Task<ShipmentDocumentDto> CreateDocumentAsync(
            CreateShipmentDocumentDto dto)
        {
            var document = new ShipmentDocument
            {
                ShipmentId = dto.ShipmentId,
                DocumentType = dto.DocumentType,
                FileName = dto.FileName,
                FilePath = dto.FilePath,
                UploadedAt = DateTime.UtcNow
            };

            _context.ShipmentDocuments.Add(document);

            await _context.SaveChangesAsync();

            return new ShipmentDocumentDto
            {
                Id = document.Id,
                ShipmentId = document.ShipmentId,
                DocumentType = document.DocumentType,
                FileName = document.FileName,
                FilePath = document.FilePath,
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