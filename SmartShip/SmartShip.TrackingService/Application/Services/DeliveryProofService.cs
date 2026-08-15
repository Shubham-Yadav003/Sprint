using Microsoft.EntityFrameworkCore;
using SmartShip.TrackingService.Application.DTOs;
using SmartShip.TrackingService.Application.Interfaces;
using SmartShip.TrackingService.Domain.Entities;
using SmartShip.TrackingService.Infrastructure.Data;

namespace SmartShip.TrackingService.Application.Services
{
    public class DeliveryProofService : IDeliveryProofService
    {
        private readonly TrackingDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public DeliveryProofService(TrackingDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<DeliveryProofDto> UploadDeliveryProofAsync(
            int shipmentId,
            string proofType,
            IFormFile file)
        {

            if (shipmentId <= 0)
            {
                throw new ArgumentException("Invalid shipment ID.");
            }

            if (string.IsNullOrWhiteSpace(proofType))
            {
                throw new ArgumentException("Proof type is required.");
            }

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required.");
            }

            var filePath = await _fileStorageService.SaveFileAsync(
               file,
               "DeliveryProofs");

            var proof = new DeliveryProof
            {
                ShipmentId = shipmentId,
                ProofType = proofType,
                FileName = file.FileName,
                FilePath = filePath,
                UploadedAt = DateTime.UtcNow
            };

            _context.DeliveryProofs.Add(proof);

            await _context.SaveChangesAsync();

            return new DeliveryProofDto
            {
                Id = proof.Id,
                ShipmentId = proof.ShipmentId,
                ProofType = proof.ProofType,
                FileName = proof.FileName,
                FilePath = proof.FilePath,
                UploadedAt = proof.UploadedAt
            };
        }

        public async Task<List<DeliveryProofDto>> GetProofsByShipmentIdAsync(
            int shipmentId)
        {
            return await _context.DeliveryProofs
                .Where(x => x.ShipmentId == shipmentId)
                .OrderByDescending(x => x.UploadedAt)
                .Select(x => new DeliveryProofDto
                {
                    Id = x.Id,
                    ShipmentId = x.ShipmentId,
                    ProofType = x.ProofType,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    UploadedAt = x.UploadedAt
                })
                .ToListAsync();
        }
    }
}