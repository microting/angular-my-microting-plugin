using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microting.DigitalOceanBase;
using Microting.DigitalOceanBase.Infrastructure.Data;
using Microting.eFormApi.BasePn;
using Microting.eFormApi.BasePn.Infrastructure.Database.Extensions;
using Microting.eFormApi.BasePn.Infrastructure.Helpers;
using Microting.eFormApi.BasePn.Infrastructure.Models.Application;
using Microting.eFormApi.BasePn.Infrastructure.Settings;
using MyMicroting.Pn.Abstractions;
using MyMicroting.Pn.Infrastructure.Data.Seed;
using MyMicroting.Pn.Infrastructure.Data.Seed.Data;
using MyMicroting.Pn.Infrastructure.Models.Settings;
using MyMicroting.Pn.Services;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MyMicroting.Pn
{
    public class EformMyMicrotingPlugin : IEformPlugin
    {
        public string Name => "My Microting Plugin";

        public string PluginId => "angular-my-microting-plugin";

        public string PluginPath => PluginAssembly().Location;

        public string PluginBaseUrl => "my-microting-pn";

        public Assembly PluginAssembly() => typeof(EformMyMicrotingPlugin).GetTypeInfo().Assembly;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILocalizationService, LocalizationService>();
            services.AddTransient<IDropletsService, DropletsService>();
            services.AddTransient<IMyMicrotingSettingsService, MyMicrotingSettingsService>();

            services.AddDigitalOceanBaseServices();
        }

        public void ConfigureOptionsServices(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigurePluginDbOptions<MyMicrotingSettings>(configuration.GetSection("MyMicrotingSettings"));
        }

        public void ConfigureDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DigitalOceanDbContext>(o => o.UseMySql(connectionString,
                    b => b.MigrationsAssembly(PluginAssembly().FullName)));

            DigitalOceanDbContextFactory contextFactory = new DigitalOceanDbContextFactory();
            using (DigitalOceanDbContext context = contextFactory.CreateDbContext(new[] { connectionString }))
            {
                context.Database.Migrate();
            }

            // Seed database
            SeedDatabase(connectionString);
        }

        public void Configure(IApplicationBuilder appBuilder)
        {
        }

        public MenuModel HeaderMenu(IServiceProvider serviceProvider)
        {
            var localizationService = serviceProvider
              .GetService<ILocalizationService>();
            var result = new MenuModel();
            result.LeftMenu.Add(new MenuItemModel()
            {
                Name = localizationService.GetString("MyMicroting"),
                E2EId = "",
                Link = "",
                Guards = new List<string>() {  }, // remove hardcode
                MenuItems = new List<MenuItemModel>()
                {
                    new MenuItemModel()
                    {
                        Name = localizationService.GetString("Organizations"),
                        E2EId = "my-microting-pn-organizations",
                        Link = "/plugins/my-microting-pn/organizations",
                        Guards = new List<string>() { },// remove hardcode
                        Position = 0,
                    },
                    new MenuItemModel()
                    {
                        Name = localizationService.GetString("Droplets"),
                        E2EId = "my-microting-pn-droplets",
                        Link = "/plugins/my-microting-pn/droplets",
                        Guards = new List<string>() {  },// remove hardcode
                        Position = 1,
                    },
                }
            });
            return result;
        }

        public void SeedDatabase(string connectionString)
        {
            DigitalOceanDbContextFactory contextFactory = new DigitalOceanDbContextFactory();
            using (var context = contextFactory.CreateDbContext(new[] { connectionString }))
                MyMicrotingPluginSeed.SeedData(context);
        }

        public void AddPluginConfig(IConfigurationBuilder builder, string connectionString)
        {
            var seedData = new MyMicrotingConfigurationSeedData();
            var contextFactory = new DigitalOceanDbContextFactory();
            builder.AddPluginConfiguration(
                connectionString,
                seedData,
                contextFactory);
        }

        public PluginPermissionsManager GetPermissionsManager(string connectionString)
        {
            var contextFactory = new DigitalOceanDbContextFactory();
            var context = contextFactory.CreateDbContext(new[] { connectionString });

            return new PluginPermissionsManager(context);
        }

    }
}
