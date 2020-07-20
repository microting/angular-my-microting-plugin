using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microting.DigitalOceanBase.Infrastructure.Data;
using Microting.eFormApi.BasePn.Infrastructure.Helpers.PluginDbOptions;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using MyMicroting.Pn.Abstractions;
using MyMicroting.Pn.Infrastructure.Models.Settings;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyMicroting.Pn.Services
{
    public class MyMicrotingSettingsService : IMyMicrotingSettingsService
    {
        private readonly ILogger<MyMicrotingSettingsService> logger;
        private readonly ILocalizationService localizationService;
        private readonly IPluginDbOptions<MyMicrotingSettings> options;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DigitalOceanDbContext dbContext;

        public MyMicrotingSettingsService(ILogger<MyMicrotingSettingsService> logger, ILocalizationService localizationService,
            IPluginDbOptions<MyMicrotingSettings> options, IHttpContextAccessor httpContextAccessor, DigitalOceanDbContext dbContext)
        {
            this.logger = logger;
            this.localizationService = localizationService;
            this.options = options;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<OperationDataResult<MyMicrotingSettings>> GetSettings()
        {
            try
            {
                var result = new MyMicrotingSettings();
                var pliginSettings = options.Value;
                if (pliginSettings?.DigitalOceanToken == null)
                    return new OperationDataResult<MyMicrotingSettings>(false, localizationService.GetString("DoTokenMissing"));

                return new OperationDataResult<MyMicrotingSettings>(true, result);

            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                logger.LogError(e.Message);
                return new OperationDataResult<MyMicrotingSettings>(false,
                    localizationService.GetString("ErrorObtainingSettingsInfo"));
            }
        }

        public async Task<OperationResult> UpdateSettings(MyMicrotingSettings settingsUpdateModel)
        {
            try
            {
                await options.UpdateDb(settings =>
                {
                    settings.DigitalOceanToken = settingsUpdateModel.DigitalOceanToken;
                }, dbContext, UserId);

                return new OperationResult(true,
                    localizationService.GetString("SettingsHaveBeenUpdatedSuccessfully"));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                logger.LogError(e.Message);
                return new OperationDataResult<MyMicrotingSettings>(false,
                    localizationService.GetString("ErrorUpdatingSettingsInfo"));
            }
        }

        public int UserId
        {
            get
            {
                var value = httpContextAccessor?.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return value == null ? 0 : int.Parse(value);
            }
        }
    }
}
