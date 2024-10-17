using Microsoft.EntityFrameworkCore;
using WEB_253502_HVESKO.Domain.Entities;

namespace WEB_253502_HVESKO.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Service> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
