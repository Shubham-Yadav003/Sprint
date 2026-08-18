using Microsoft.EntityFrameworkCore;
using SmartShip.AdminService.Application.DTOs;
using SmartShip.AdminService.Application.Interfaces;
using SmartShip.AdminService.Domain.Entities;
using SmartShip.AdminService.Infrastructure.Data;

namespace SmartShip.AdminService.Application.Services
{
    public class IssueService : IIssueService
    {
        private readonly AdminDbContext _context;

        public IssueService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<DeliveryIssueDto> CreateIssueAsync(CreateDeliveryIssueDto dto)
        {
            var issue = new DeliveryIssue
            {
                ShipmentId = dto.ShipmentId,
                IssueType = dto.IssueType,
                Description = dto.Description,
                Status = "Open",
                CreatedAt = DateTime.UtcNow
            };

            _context.DeliveryIssues.Add(issue);
            await _context.SaveChangesAsync();

            return new DeliveryIssueDto
            {
                Id = issue.Id,
                ShipmentId = issue.ShipmentId,
                IssueType = issue.IssueType,
                Description = issue.Description,
                Status = issue.Status,
                CreatedAt = issue.CreatedAt
            };
        }

        public async Task<IEnumerable<DeliveryIssueDto>> GetAllIssuesAsync(string? status = null)
        {
            var query = _context.DeliveryIssues.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(i => i.Status.ToLower() == status.ToLower());
            }

            return await query
                .Select(i => new DeliveryIssueDto
                {
                    Id = i.Id,
                    ShipmentId = i.ShipmentId,
                    IssueType = i.IssueType,
                    Description = i.Description,
                    Status = i.Status,
                    ResolutionNotes = i.ResolutionNotes,
                    CreatedAt = i.CreatedAt,
                    ResolvedAt = i.ResolvedAt
                })
                .ToListAsync();
        }

        public async Task<bool> ResolveIssueAsync(int id, ResolveIssueDto dto)
        {
            var issue = await _context.DeliveryIssues.FindAsync(id);
            if (issue == null) return false;

            issue.Status = "Resolved";
            issue.ResolutionNotes = dto.ResolutionNotes;
            issue.ResolvedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}