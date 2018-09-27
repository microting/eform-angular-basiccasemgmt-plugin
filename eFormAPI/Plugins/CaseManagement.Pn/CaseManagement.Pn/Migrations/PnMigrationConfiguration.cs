using CaseManagement.Pn.Infrastructure.Data;

namespace CaseManagement.Pn.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class PnMigrationConfiguration : DbMigrationsConfiguration<CaseManagementPnDbContext>
    {
        public PnMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CaseManagementPnDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
