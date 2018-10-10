using CaseManagement.Pn.Abstractions;
using CaseManagement.Pn.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Controllers
{
    [Authorize]
    public class CaseManagementSettingsController : Controller
    {
        private readonly ICaseManagementSettingsService _caseManagementSettingsService;

        public CaseManagementSettingsController(ICaseManagementSettingsService caseManagementSettingsService)
        {
            _caseManagementSettingsService = caseManagementSettingsService;
        }

        [HttpGet]
        [Authorize(Roles = EformRole.Admin)]
        [Route("api/case-management-pn/settings")]
        public OperationDataResult<CaseManagementPnSettingsModel> GetSettings()
        {
            return _caseManagementSettingsService.GetSettings();
        }

        [HttpPost]
        [Authorize(Roles = EformRole.Admin)]
        [Route("api/case-management-pn/settings")]
        public OperationResult UpdateSettings(CaseManagementPnSettingsModel caseManagementSettingsModel)
        {
            return _caseManagementSettingsService.UpdateSettings(caseManagementSettingsModel);
        }
    }
}