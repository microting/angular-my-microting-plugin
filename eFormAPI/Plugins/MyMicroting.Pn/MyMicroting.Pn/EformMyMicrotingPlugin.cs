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
using Microting.eFormBaseCustomerBase.Infrastructure.Data;
using Microting.eFormBaseCustomerBase.Infrastructure.Data.Entities;
using Microting.eFormBaseCustomerBase.Infrastructure.Data.Factories;
using Microting.MyMicrotingBase.Infrastructure.Data;
using MyMicroting.Pn.Abstractions;
using MyMicroting.Pn.Infrastructure.Data.Seed;
using MyMicroting.Pn.Infrastructure.Data.Seed.Data;
using MyMicroting.Pn.Infrastructure.Extensions;
using MyMicroting.Pn.Infrastructure.Models.Settings;
using MyMicroting.Pn.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
            services.AddTransient<IOrganizationsService, OrganizationsService>();
            services.AddTransient<IMyMicrotingSettingsService, MyMicrotingSettingsService>();

            services.AddDigitalOceanBaseServices();
        }

        public void ConfigureOptionsServices(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigurePluginDbOptions<MyMicrotingSettings>(configuration.GetSection("MyMicrotingSettings"));
        }

        public void ConfigureDbContext(IServiceCollection services, string connectionString)
        {
            var customersConnectionString = connectionString.Replace("angular-my-microting-plugin", "eform-angular-basecustomer-plugin");
            var orgConnectionString = connectionString.Replace("angular-my-microting-plugin", "angular-my-microting-organizations-plugin");

            // todo: connection string, seed customers db, same for my microting db context
            services.AddDbContext<DigitalOceanDbContext>(o => o.UseMySql(connectionString,
                b => b.MigrationsAssembly(PluginAssembly().FullName)));

            services.AddDbContext<CustomersPnDbAnySql>(o => o.UseMySql(customersConnectionString,
                 b => b.MigrationsAssembly(PluginAssembly().FullName)));

            services.AddDbContext<MyMicrotingDbContext>(o => o.UseMySql(orgConnectionString,
                 b => b.MigrationsAssembly(PluginAssembly().FullName)));

            DigitalOceanDbContextFactory contextFactory = new DigitalOceanDbContextFactory();
            using (DigitalOceanDbContext context = contextFactory.CreateDbContext(new[] { connectionString }))
                context.Database.Migrate();

            CustomersPnContextFactory customersPnContextFactory = new CustomersPnContextFactory();
            using (CustomersPnDbAnySql context = customersPnContextFactory.CreateDbContext(new[] { customersConnectionString }))
                context.Database.Migrate();

            MyMicrotingDbContextFactory myMicrotingDbContextFactory = new MyMicrotingDbContextFactory();
            using (MyMicrotingDbContext context = myMicrotingDbContextFactory.CreateDbContext(new[] { orgConnectionString }))
                context.Database.Migrate();

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
            var customersConnectionString = connectionString.Replace("angular-my-microting-plugin", "eform-angular-basecustomer-plugin");
            var orgConnectionString = connectionString.Replace("angular-my-microting-plugin", "angular-my-microting-organizations-plugin");

            DigitalOceanDbContextFactory contextFactory = new DigitalOceanDbContextFactory();
            using (var context = contextFactory.CreateDbContext(new[] { connectionString }))
                MyMicrotingPluginSeed.SeedData(context, new MyMicrotingDropletsConfigurationSeedData());

            MyMicrotingDbContextFactory myMicrotingDbContextFactory = new MyMicrotingDbContextFactory();
            using (var context = myMicrotingDbContextFactory.CreateDbContext(new[] { orgConnectionString }))
                MyMicrotingPluginSeed.SeedData(context, new MyMicrotingOrganizationsConfigurationSeedData());

            CustomersPnContextFactory customersPnContextFactory = new CustomersPnContextFactory();
            using (CustomersPnDbAnySql context = customersPnContextFactory.CreateDbContext(new[] { customersConnectionString }))
            {
                // Add data
                List<string>
                    customerFields =
                        new Customer().GetPropList(); //Find all attributes for cusomers and puts them in a list
                customerFields.Remove(nameof(Customer
                    .RelatedEntityId)); // removes the related entity, because it's not relevant for fields
                foreach (string name in customerFields)
                {
                    if (!context.Fields.Any(x => x.Name == name))
                    {
                        Field newField = new Field
                        {
                            Name = name
                        };
                        newField.Create(context);
                    }
                }

                context.SaveChanges();
                Field fieldForRemove = context.Fields.FirstOrDefault(x => x.Name == nameof(Customer.RelatedEntityId));
                if (fieldForRemove != null)
                {
                    context.Fields.Remove(fieldForRemove);
                    context.SaveChanges();
                }

                List<Field> fields = context.Fields.ToList();
                foreach (Field field in fields)
                {
                    CustomerField customerField = new CustomerField
                    {
                        FieldId = field.Id,
                        FieldStatus = 1
                    };
                    if (!context.CustomerFields.Any(x => x.FieldId == field.Id))
                    {
                        context.CustomerFields.Add(customerField);
                    }
                }

                context.SaveChanges();

                // Seed configuration
                MyMicrotingPluginSeed.SeedData(context, new MyMicrotingCustomersConfigurationSeedData());
                MyMicrotingPluginSeed.SeedPermissions(context, new MyMicrotingCustomersPermissionsSeedData());
            }
        }

        public void AddPluginConfig(IConfigurationBuilder builder, string connectionString)
        {
            var seedData = new MyMicrotingDropletsConfigurationSeedData();
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
