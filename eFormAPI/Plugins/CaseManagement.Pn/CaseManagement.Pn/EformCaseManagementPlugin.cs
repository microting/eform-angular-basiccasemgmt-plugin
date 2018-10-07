using System.Reflection;
using CaseManagement.Pn.Abstractions;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Data.Factories;
using CaseManagement.Pn.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microting.eFormApi.BasePn;

namespace CaseManagement.Pn
{
    public class EformCaseManagementPlugin : IEformPlugin
    {
        public string GetName() => "Microting Case Management plugin";
        public string ConnectionStringName() => "EFormCaseManagementPnConnection";
        public string PluginPath() => PluginAssembly().Location;

        public Assembly PluginAssembly()
        {
            return typeof(EformCaseManagementPlugin).GetTypeInfo().Assembly;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICaseManagementLocalizationService, CaseManagementLocalizationService>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<ICalendarUsersService, CalendarUsersService>();
            services.AddScoped<ICaseManagementSettingsService, CaseManagementSettingsService>();
        }

        public void ConfigureDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CaseManagementPnDbContext>(o => o.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(PluginAssembly().FullName)));

            var contextFactory = new CaseManagementPnDbContextFactory();
            var context = contextFactory.CreateDbContext(new[] {connectionString});
            context.Database.Migrate();
        }

        public void Configure(IApplicationBuilder appBuilder)
        {
        }
        
        public void SeedDatabase(string connectionString)
        {
        }
    }
}
