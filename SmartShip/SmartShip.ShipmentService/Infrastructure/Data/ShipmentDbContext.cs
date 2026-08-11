using Microsoft.EntityFrameworkCore;
using SmartShip.ShipmentService.Domain.Entities;
namespace SmartShip.ShipmentService.Infrastructure.Data
{
    public class ShipmentDbContext: DbContext
    {
        public ShipmentDbContext(DbContextOptions<ShipmentDbContext> options): base(options)
        {

        }

        public DbSet<Shipment> Shipments { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Package> Packages { get; set; }
    }
}
