using CaseManagement.Pn.Infrastructure.Const;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

namespace CaseManagement.Pn.Infrastructure.Data.Seed.Data
{
    public static class CaseManagementPermissionsSeedData
    {
        public static PluginPermission[] Data => new[]
        {
            new PluginPermission()
            {
                PermissionName = "Access CaseManagement Plugin",
                ClaimName = CaseManagementClaims.AccessCaseManagementPlugin
            },
            new PluginPermission()
            {
                PermissionName = "Create Calendar Users",
                ClaimName = CaseManagementClaims.CreateCalendarUsers
            },
        };
    }
}