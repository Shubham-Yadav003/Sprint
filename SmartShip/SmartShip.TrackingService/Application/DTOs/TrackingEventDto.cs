namespace SmartShip.TrackingService.Application.DTOs
{
    public class TrackingEventDto
    {
        public int Id { get; set; }

        public int ShipmentId { get; set; }

        public string Status { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
