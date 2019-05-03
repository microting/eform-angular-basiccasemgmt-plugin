using System.Threading.Tasks;
using CaseManagement.Pn.Infrastructure.Models;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Abstractions
{
    public interface ICaseManagementSettingsService
    {
        Task<OperationDataResult<CaseManagementBaseSettings>> GetSettings();
        Task<OperationResult> UpdateSettings(CaseManagementBaseSettings caseManagementSettingsModel);
    }
}