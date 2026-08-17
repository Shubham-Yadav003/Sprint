using Microsoft.EntityFrameworkCore;
using SmartShip.AdminService.Domain.Entities;
namespace SmartShip.AdminService.Infrastructure.Data
{
    public class AdminDbContext: DbContext
    {

        public AdminDbContext(DbContextOptions<AdminDbContext> options): base(options)
        {

        }

        public DbSet<Location>Locations { get; set; }
    }
}
//It connects our Location entity to EF Core.
