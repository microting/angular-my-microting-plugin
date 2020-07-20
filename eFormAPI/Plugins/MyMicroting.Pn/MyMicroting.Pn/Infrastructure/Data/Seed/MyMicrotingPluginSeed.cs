using Microting.DigitalOceanBase.Infrastructure.Data;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using Microting.eForm.Infrastructure.Constants;
using System;
using System.Linq;
using MyMicroting.Pn.Infrastructure.Data.Seed.Data;

namespace MyMicroting.Pn.Infrastructure.Data.Seed
{
    public class MyMicrotingPluginSeed
    {
        public static void SeedData(DigitalOceanDbContext dbContext)
        {
            var seedData = new MyMicrotingConfigurationSeedData();
            var configurationList = seedData.Data;
            foreach (var configurationItem in configurationList)
            {
                if (!dbContext.PluginConfigurationValues.Any(x => x.Name == configurationItem.Name))
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

            //// Seed plugin permissions
            //var newPermissions = CustomersPermissionsSeedData.Data
            //    .Where(p => dbContext.PluginPermissions.All(x => x.ClaimName != p.ClaimName))
            //    .Select(p => new PluginPermission
            //    {
            //        PermissionName = p.PermissionName,
            //        ClaimName = p.ClaimName,
            //        CreatedAt = DateTime.UtcNow,
            //        Version = 1,
            //        WorkflowState = Constants.WorkflowStates.Created,
            //        CreatedByUserId = 1
            //    }
            //    );
            //dbContext.PluginPermissions.AddRange(newPermissions);

            dbContext.SaveChanges();
        }
    }
}
