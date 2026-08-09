using Microsoft.EntityFrameworkCore;
using SmartShip.IdentityService.Domain.Entities;
namespace SmartShip.IdentityService.Infrastructure.Data
{
    public class IdentityDbContext: DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
