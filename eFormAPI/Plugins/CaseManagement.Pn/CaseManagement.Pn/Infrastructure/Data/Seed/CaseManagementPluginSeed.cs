using CaseManagement.Pn.Infrastructure.Data.Seed.Data;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using System;
using System.Linq;
using Microting.eForm.Infrastructure.Constants;
using Microting.eFormBasicCaseManagementBase.Infrastructure.Data;

namespace CaseManagement.Pn.Infrastructure.Data.Seed
{
    public class CaseManagementPluginSeed
    {
        public static void SeedData(eFormCaseManagementPnDbContext dbContext)
        {
            var seedData = new CaseManagementConfigurationSeedData();
            var configurationList = seedData.Data;
            foreach (var configurationItem in configurationList)
            {
                if (!dbContext.PluginConfigurationValues.Any(x=>x.Name == configurationItem.Name))
                {
                    var newConfigValue = new PluginConfigurationValue()
                    {
                        Name = configurationItem.Name,
                        Value = configurationItem.Value,
                        CreatedAt = DateTime.UtcNow,
                        Version = 1,
                        WorkflowState = Constants.WorkflowStates.Created,
                        CreatedByUserId = 1
                    };
                    dbContext.PluginConfigurationValues.Add(newConfigValue);
                    dbContext.SaveChanges();
                }
            }

            // Seed plugin permissions
            var newPermissions = CaseManagementPermissionsSeedData.Data
                .Where(p => dbContext.PluginPermissions.All(x => x.ClaimName != p.ClaimName))
                .Select(p => new PluginPermission
                {
                    PermissionName = p.PermissionName,
                    ClaimName = p.ClaimName,
                    CreatedAt = DateTime.UtcNow,
                    Version = 1,
                    WorkflowState = Constants.WorkflowStates.Created,
                    CreatedByUserId = 1
                }
                );
            dbContext.PluginPermissions.AddRange(newPermissions);

            dbContext.SaveChanges();
        }
    }
}