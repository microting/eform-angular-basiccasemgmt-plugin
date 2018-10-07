using CaseManagement.Pn.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseManagement.Pn.Infrastructure.Data
{
    public class CaseManagementPnDbContext : DbContext
    {
        public CaseManagementPnDbContext(DbContextOptions<CaseManagementPnDbContext> options) 
            : base(options)
        {
        }

        public DbSet<CaseManagementSetting> CaseManagementSettings { get; set; }
        public DbSet<CalendarUser> CalendarUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CalendarUser>()
                .HasIndex(x => x.SiteId)
                .IsUnique();
        }
    }
}
