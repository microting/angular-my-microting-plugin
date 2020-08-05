using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

namespace MyMicroting.Pn.Infrastructure.Data.Seed.Data
{
    public interface IPluginPermissionsSeedData
    {
        public PluginPermission[] Data { get; }
    }
}
