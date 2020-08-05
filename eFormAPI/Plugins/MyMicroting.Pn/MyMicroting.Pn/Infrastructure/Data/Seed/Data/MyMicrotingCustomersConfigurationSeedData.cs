using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

namespace MyMicroting.Pn.Infrastructure.Data.Seed.Data
{
    public class MyMicrotingCustomersConfigurationSeedData : IPluginConfigurationSeedData
    {
        public PluginConfigurationValue[] Data => new[]
        {
            new PluginConfigurationValue()
            {
                Name = "CustomersSettings:RelatedEntityGroupId",
                Value = ""
            },
        };
    }
}
