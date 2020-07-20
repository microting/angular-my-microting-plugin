using Microsoft.Extensions.Localization;
using Microting.eFormApi.BasePn.Localization.Abstractions;
using MyMicroting.Pn.Abstractions;

namespace MyMicroting.Pn.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IStringLocalizer _localizer;

        // ReSharper disable once SuggestBaseTypeForParameter
        public LocalizationService(IEformLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(EformMyMicrotingPlugin));
        }

        public string GetString(string key)
        {
            var str = _localizer.GetString(key);
            return str.Value;
        }

        public string GetString(string format, params object[] args)
        {
            var message = _localizer[format];
            if (message?.Value == null)
            {
                return null;
            }

            return string.Format(message.Value, args);
        }
    }
}