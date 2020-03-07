using Microsoft.EntityFrameworkCore;
using MyData.Core.Models;

namespace MyData.Infrastructure.EntityFrameworkCore
{
    public class AppDbContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Log> Logs { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}