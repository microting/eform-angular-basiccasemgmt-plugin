using CaseManagement.Pn.Abstractions;
using Microsoft.Extensions.Localization;
using Microting.eFormApi.BasePn.Localization.Abstractions;

namespace CaseManagement.Pn.Services
{
    public class CaseManagementLocalizationService : ICaseManagementLocalizationService
    {
        private readonly IStringLocalizer _localizer;
 
        public CaseManagementLocalizationService(IEformLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(EformCaseManagementPlugin));
        }
 
        public string GetString(string key)
        {
            var str = _localizer[key];
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
