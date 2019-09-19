using System;
using System.Collections.Generic;
using System.Reflection;
using CaseManagement.Pn.Abstractions;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Data.Factories;
using CaseManagement.Pn.Infrastructure.Data.Seed;
using CaseManagement.Pn.Infrastructure.Data.Seed.Data;
using CaseManagement.Pn.Infrastructure.Models;
using CaseManagement.Pn.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microting.eFormApi.BasePn;
using Microting.eFormApi.BasePn.Infrastructure.Database.Extensions;
using Microting.eFormApi.BasePn.Infrastructure.Models.Application;
using Microting.eFormApi.BasePn.Infrastructure.Settings;

namespace CaseManagement.Pn
{
    public class EformCaseManagementPlugin : IEformPlugin
    {
        public string Name => "Microting Case Management plugin";
        public string PluginId => "eform-angular-basiccasemgmt-plugin";
        public string PluginPath => PluginAssembly().Location;
        private string _connectionString;

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

        public void AddPluginConfig(IConfigurationBuilder builder, string connectionString)
        {
            var seedData = new CaseManagementConfigurationSeedData();
            var contextFactory = new CaseManagementPnDbContextFactory();
            builder.AddPluginConfiguration(
                connectionString,
                seedData,
                contextFactory);
        }

        public void ConfigureOptionsServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigurePluginDbOptions<CaseManagementBaseSettings>(
                configuration.GetSection("CaseManagementBaseSettings"));
        }
        public void ConfigureDbContext(IServiceCollection services, string connectionString)
        {
            _connectionString = connectionString;
            if (connectionString.ToLower().Contains("convert zero datetime"))
            {                
                services.AddDbContext<CaseManagementPnDbContext>(o => o.UseMySql(connectionString,
                    b => b.MigrationsAssembly(PluginAssembly().FullName)));
            }
            else
            {                
                services.AddDbContext<CaseManagementPnDbContext>(o => o.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(PluginAssembly().FullName)));
            }

            var contextFactory = new CaseManagementPnDbContextFactory();
            var context = contextFactory.CreateDbContext(new[] {connectionString});
            context.Database.Migrate();
            
            SeedDatabase(connectionString);
        }

        public void Configure(IApplicationBuilder appBuilder)
        {
            var serviceProvider = appBuilder.ApplicationServices;
        }

        public MenuModel HeaderMenu(IServiceProvider serviceProvider)
        {
            var localizationService = serviceProvider
                .GetService<ICaseManagementLocalizationService>();

            var result = new MenuModel();
            result.LeftMenu.Add(new MenuItemModel()
            {
                Name = localizationService.GetString("CaseManagement"),
                E2EId = "case-management-pn",
                Link = "",
                MenuItems = new List<MenuItemModel>()
                {
                    new MenuItemModel()
                    {
                        Name = localizationService.GetString("Calendar"),
                        E2EId = "case-management-pn-calendar",
                        Link = "/plugins/case-management-pn/calendar",
                        Position = 0,
                    },
                    new MenuItemModel()
                    {
                        Name = localizationService.GetString("Cases"),
                        E2EId = "case-management-pn-cases",
                        Link = "/plugins/case-management-pn/cases",
                        Position = 1,
                    },
                    new MenuItemModel()
                    {
                        Name = localizationService.GetString("Settings"),
                        E2EId = "case-management-pn-settings",
                        Link = "/plugins/case-management-pn/settings",
                        Position = 2,
                        Guards = new List<string>()
                        {
                            "admin"
                        }
                    },
                } 
            });
            return result;
        }

        public void SeedDatabase(string connectionString)
        {
            var contextFactory = new CaseManagementPnDbContextFactory();
            using (var context = contextFactory.CreateDbContext(new []{connectionString}))
            {
                CaseManagementPluginSeed.SeedData(context);
            }
        }

    }
}
