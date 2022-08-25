using Microsoft.EntityFrameworkCore;

namespace Covid19Chart.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Covid> Covids { get; set; }
    }
}
