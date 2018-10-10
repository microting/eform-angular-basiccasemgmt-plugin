using System.Reflection;
using CaseManagement.Pn.Abstractions;
using Microsoft.Extensions.Localization;

namespace CaseManagement.Pn.Services
{
    public class CaseManagementLocalizationService : ICaseManagementLocalizationService
    {
        private readonly IStringLocalizer _localizer;
 
        public CaseManagementLocalizationService(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create("CaseManagementResources",
                Assembly.GetEntryAssembly().FullName);
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
