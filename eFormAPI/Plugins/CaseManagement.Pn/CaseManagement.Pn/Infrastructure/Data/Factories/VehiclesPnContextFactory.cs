using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CaseManagement.Pn.Infrastructure.Data.Factories
{
    public class VehiclesPnContextFactory : IDesignTimeDbContextFactory<VehiclesPnDbContext>
    {
        public VehiclesPnDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VehiclesPnDbContext>();
            if (args.Any())
            {
                optionsBuilder.UseSqlServer(args.FirstOrDefault());
            }
            else
            {
                optionsBuilder.UseSqlServer("...");
            }
            return new VehiclesPnDbContext(optionsBuilder.Options);
        }
    }
}