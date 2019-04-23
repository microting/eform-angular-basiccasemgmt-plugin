using System;
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
                if (args.FirstOrDefault().ToLower().Contains("convert zero datetime"))
                {
                    optionsBuilder.UseMySql(args.FirstOrDefault());
                }
                else
                {
                    optionsBuilder.UseSqlServer(args.FirstOrDefault());
                }
            }
            else
            {
                throw new ArgumentNullException("Connection string not present");
            }
            optionsBuilder.UseLazyLoadingProxies(true);
            return new CaseManagementPnDbContext(optionsBuilder.Options);
        }
    }
}
