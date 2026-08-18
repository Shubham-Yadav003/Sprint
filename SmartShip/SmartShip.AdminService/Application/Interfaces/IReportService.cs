using SmartShip.AdminService.Application.DTOs;

namespace SmartShip.AdminService.Application.Interfaces
{
    public interface IReportService
    {
        Task<OperationalSummaryDto> GetOperationalSummaryAsync();
    }
}