using CaseManagement.Pn.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using Microting.eFormApi.BasePn.Infrastructure.Database.Extensions;

namespace CaseManagement.Pn.Infrastructure.Data
{
    public class CaseManagementPnDbAnySql : DbContext, IPluginDbContext
    {
        public CaseManagementPnDbAnySql() { }

        public CaseManagementPnDbAnySql(DbContextOptions<CaseManagementPnDbAnySql> options) : base(options)
        {

        }
        public DbSet<CaseManagementSetting> CaseManagementSettings { get; set; }
        public DbSet<CalendarUser> CalendarUsers { get; set; }
        public DbSet<CalendarUserVersions> CalendarUserVersions { get; set; }

        // plugin configuration
        public DbSet<PluginConfigurationValue> PluginConfigurationValues { get; set; }
        public DbSet<PluginConfigurationValueVersion> PluginConfigurationValueVersions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CalendarUser>()
                .HasIndex(x => x.SiteId)
                .IsUnique();

            modelBuilder.AddPluginSettingsRules();
        }
    }
}
