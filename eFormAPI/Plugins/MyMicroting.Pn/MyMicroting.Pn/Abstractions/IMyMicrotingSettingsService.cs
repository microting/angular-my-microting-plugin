using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using MyMicroting.Pn.Infrastructure.Models.Settings;
using System.Threading.Tasks;

namespace MyMicroting.Pn.Abstractions
{
    public interface IMyMicrotingSettingsService
    {
        Task<OperationDataResult<MyMicrotingSettings>> GetSettings();
        Task<OperationResult> UpdateSettings(MyMicrotingSettings settingsUpdateModel);
    }
}
