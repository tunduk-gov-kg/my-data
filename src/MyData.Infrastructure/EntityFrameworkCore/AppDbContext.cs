using Microsoft.EntityFrameworkCore;
using MyData.Core.Models;

namespace MyData.Infrastructure.EntityFrameworkCore
{
    public class AppDbContext : DbContext
    {
        public DbSet<XRoadRequest> XRoadRequests { get; set; }

        public DbSet<XRoadService> XRoadServices { get; set; }

        public DbSet<JournalRecord> Journal { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}