using SmartShip.AdminService.Application.DTOs;

namespace SmartShip.AdminService.Application.Interfaces
{
    public interface IIssueService
    {
        Task<DeliveryIssueDto> CreateIssueAsync(CreateDeliveryIssueDto dto);

        Task<IEnumerable<DeliveryIssueDto>> GetAllIssuesAsync(string? status = null);


        Task<bool> ResolveIssueAsync(int id, ResolveIssueDto dto);
    }
}
