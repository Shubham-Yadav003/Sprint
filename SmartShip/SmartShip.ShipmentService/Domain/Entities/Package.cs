namespace SmartShip.ShipmentService.Domain.Entities
{
    public class Package
    {
        public int Id { get; set; }

        public int ShipmentId { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Weight { get; set; }
    }
}
