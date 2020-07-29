using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using Microting.eFormBaseCustomerBase.Infrastructure.Const;

namespace MyMicroting.Pn.Infrastructure.Data.Seed.Data
{
    public class MyMicrotingCustomersPermissionsSeedData: IPluginPermissionsSeedData
    {
        public PluginPermission[] Data => new[]
        {
            new PluginPermission()
            {
                PermissionName = "Access BaseCustomer Plugin",
                ClaimName = CustomersClaims.AccessCustomersPlugin
            },
            new PluginPermission()
            {
                PermissionName = "Create Customers",
                ClaimName = CustomersClaims.CreateCustomers
            },
        };
    }
}
