using CaseManagement.Pn.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using Microting.eFormApi.BasePn.Infrastructure.Database.Extensions;

namespace CaseManagement.Pn.Infrastructure.Data
{
    public class CaseManagementPnDbContext : DbContext, IPluginDbContext
    {
        public CaseManagementPnDbContext() { }

        public CaseManagementPnDbContext(DbContextOptions<CaseManagementPnDbContext> options) : base(options)
        {

        }
        public DbSet<CalendarUser> CalendarUsers { get; set; }
        public DbSet<CalendarUserVersions> CalendarUserVersions { get; set; }

        // plugin configuration
        public DbSet<PluginConfigurationValue> PluginConfigurationValues { get; set; }
        public DbSet<PluginConfigurationValueVersion> PluginConfigurationValueVersions { get; set; }
        public DbSet<PluginPermission> PluginPermissions { get; set; }
        public DbSet<PluginGroupPermission> PluginGroupPermissions { get; set; }
        public DbSet<PluginGroupPermissionVersion> PluginGroupPermissionVersions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CalendarUser>()
                .HasIndex(x => x.SiteId)
                .IsUnique();

            modelBuilder.Entity<PluginGroupPermissionVersion>()
                .HasOne(x => x.PluginGroupPermission)
                .WithMany()
                .HasForeignKey("FK_PluginGroupPermissionVersions_PluginGroupPermissionId")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.AddPluginSettingsRules();
        }
    }
}
