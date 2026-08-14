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

        public DeliveryProofService(TrackingDbContext context)
        {
            _context = context;
        }

        public async Task<DeliveryProofDto> CreateDeliveryProofAsync(
            CreateDeliveryProofDto dto)
        {
            var proof = new DeliveryProof
            {
                ShipmentId = dto.ShipmentId,
                ProofType = dto.ProofType,
                FileName = dto.FileName,
                FilePath = dto.FilePath,
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