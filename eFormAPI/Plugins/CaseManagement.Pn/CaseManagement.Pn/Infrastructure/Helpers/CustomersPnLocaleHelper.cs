using System.Threading;
using CaseManagement.Pn.Properties;

namespace CaseManagement.Pn.Infrastructure.Helpers
{
    public static class CustomersPnLocaleHelper
    {
        public static string GetString(string str)
        {
            var message = CaseManagementPnResources.ResourceManager.GetString(str, Thread.CurrentThread.CurrentCulture);
            return message;
        }

        public static string GetString(string format, params object[] args)
        {
            var message = CaseManagementPnResources.ResourceManager.GetString(format, Thread.CurrentThread.CurrentCulture);
            if (message == null)
            {
                return null;
            }
            message = string.Format(message, args);
            return message;
        }
    }
}
