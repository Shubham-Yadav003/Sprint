using Microsoft.EntityFrameworkCore;
using SmartShip.TrackingService.Domain.Entities;
namespace SmartShip.TrackingService.Infrastructure.Data
{
    public class TrackingDbContext: DbContext
    {
        public TrackingDbContext(DbContextOptions<TrackingDbContext> options)
           : base(options)
        {
        }

        public DbSet<TrackingEvent> TrackingEvents { get; set; }

        public DbSet<ShipmentDocument> ShipmentDocuments { get; set; }

        public DbSet<DeliveryProof> DeliveryProofs { get; set; }

    }
}
