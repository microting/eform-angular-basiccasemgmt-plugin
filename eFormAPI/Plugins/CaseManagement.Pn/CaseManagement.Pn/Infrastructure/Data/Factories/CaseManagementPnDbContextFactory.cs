using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CaseManagement.Pn.Infrastructure.Data.Factories
{
    public class CaseManagementPnDbContextFactory : IDesignTimeDbContextFactory<CaseManagementPnDbContext>
    {
        public CaseManagementPnDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CaseManagementPnDbContext>();
            if (args.Any())
            {
                optionsBuilder.UseSqlServer(args.FirstOrDefault());
            }
            else
            {
                optionsBuilder.UseSqlServer("...");
            }
            return new CaseManagementPnDbContext(optionsBuilder.Options);
        }
    }
}