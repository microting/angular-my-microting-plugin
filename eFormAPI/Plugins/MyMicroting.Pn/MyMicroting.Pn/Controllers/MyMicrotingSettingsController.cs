using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using MyMicroting.Pn.Abstractions;
using MyMicroting.Pn.Infrastructure.Models.Settings;
using System.Threading.Tasks;

namespace MyMicroting.Pn.Controllers
{
    //[Authorize(Roles = EformRole.Admin)]
    public class MyMicrotingSettingsController : Controller
    {
        private readonly IMyMicrotingSettingsService myMicrotingSettingsService;

        public MyMicrotingSettingsController(IMyMicrotingSettingsService myMicrotingSettingsService)
        {
            this.myMicrotingSettingsService = myMicrotingSettingsService;
        }

        [HttpGet]
        [Route("api/my-microting-pn/settings")]
        public async Task<OperationDataResult<MyMicrotingSettings>> GetSettings()
        {
            return await myMicrotingSettingsService.GetSettings();
        }


        [HttpPost]
        [Route("api/my-microting-pn/settings")]
        public async Task<OperationResult> UpdateSettings([FromBody] MyMicrotingSettings settingsUpdateModel)
        {
            return await myMicrotingSettingsService.UpdateSettings(settingsUpdateModel);
        }
    }
}
