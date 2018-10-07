using CaseManagement.Pn.Infrastructure.Models;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Abstractions
{
    public interface ICaseManagementSettingsService
    {
        OperationDataResult<CaseManagementPnSettingsModel> GetSettings();
        OperationResult UpdateSettings(CaseManagementPnSettingsModel caseManagementSettingsModel);
    }
}