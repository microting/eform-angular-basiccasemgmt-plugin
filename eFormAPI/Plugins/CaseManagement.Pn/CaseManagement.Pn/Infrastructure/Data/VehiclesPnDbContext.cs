using CaseManagement.Pn.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseManagement.Pn.Infrastructure.Data
{
    public class VehiclesPnDbContext : DbContext
    {
        public VehiclesPnDbContext(DbContextOptions<VehiclesPnDbContext> options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
                .HasIndex(x => x.VinNumber)
                .IsUnique();
        }
    }
}