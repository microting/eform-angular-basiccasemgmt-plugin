using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

namespace CaseManagement.Pn.Infrastructure.Data.Seed.Data
{
    public class CaseManagementConfigurationSeedData : IPluginConfigurationSeedData
    {
        public PluginConfigurationValue[] Data => new[]
        {
            new PluginConfigurationValue()
            {
                Name = "CaseManagementBaseSettings:LogLevel",
                Value = "4"
            },
            new PluginConfigurationValue()
            {
                Name = "CaseManagementBaseSettings:LogLimit",
                Value = "25000"
            },
            new PluginConfigurationValue()
            {
                Name = "CaseManagementBaseSettings:SdkConnectionString",
                Value = "..."
            },
            new PluginConfigurationValue()
            {
                Name = "CaseManagementBaseSettings:MaxParallelism",
                Value = "1"
            },
            new PluginConfigurationValue()
            {
                Name = "CaseManagementBaseSettings:NumberOfWorkers",
                Value = "1"
            },
            new PluginConfigurationValue()
            {
                Name = "CaseManagementBaseSettings:SelectedTemplateId",
                Value = "0"
            },
            new PluginConfigurationValue()
            {
                Name = "CaseManagementBaseSettings:RelatedEntityGroupId",
                Value = "0"
            }
            
        };
    }
}