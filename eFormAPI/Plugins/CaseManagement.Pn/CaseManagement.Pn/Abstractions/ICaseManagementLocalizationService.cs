namespace CaseManagement.Pn.Abstractions
{
    public interface ICaseManagementLocalizationService
    {
        string GetString(string key);
        string GetString(string format, params object[] args);
    }
}