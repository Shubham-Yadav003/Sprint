namespace SmartShip.AdminService.Application.DTOs
{
    public class LocationMetricsDto
    {
        public int Total { get; set; }
        public int Active { get; set; }
        public int Inactive { get; set; }
    }

    public class IssueMetricsDto
    {
        public int Total { get; set; }
        public int Open { get; set; }
        public int Resolved { get; set; }
    }

    public class OperationalSummaryDto
    {
        public LocationMetricsDto Locations { get; set; } = new();
        public IssueMetricsDto DeliveryIssues { get; set; } = new();
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}