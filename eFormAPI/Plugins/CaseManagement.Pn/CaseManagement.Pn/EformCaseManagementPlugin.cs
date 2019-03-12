using System;
using System.Collections.Generic;
using System.Reflection;
using CaseManagement.Pn.Abstractions;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Data.Factories;
using CaseManagement.Pn.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microting.eFormApi.BasePn;
using Microting.eFormApi.BasePn.Infrastructure.Models.Application;

namespace CaseManagement.Pn
{
    public class EformCaseManagementPlugin : IEformPlugin
    {
        public string Name => "Microting Case Management plugin";
        public string PluginId => "EFormCaseManagementPn";
        public string PluginPath => PluginAssembly().Location;

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
        }

        public void ConfigureDbContext(IServiceCollection services, string connectionString)
        {
            if (connectionString.ToLower().Contains("convert zero datetime"))
            {                
                services.AddDbContext<CaseManagementPnDbAnySql>(o => o.UseMySql(connectionString,
                    b => b.MigrationsAssembly(PluginAssembly().FullName)));
            }
            else
            {                
                services.AddDbContext<CaseManagementPnDbAnySql>(o => o.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(PluginAssembly().FullName)));
            }

            var contextFactory = new CaseManagementPnDbContextFactory();
            var context = contextFactory.CreateDbContext(new[] {connectionString});
            context.Database.Migrate();
        }

        public void Configure(IApplicationBuilder appBuilder)
        {
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
        }

        public void ConfigureOptionsServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
        }
    }
}
