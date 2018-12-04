using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Data.Entities;
using CaseManagement.Pn.Infrastructure.Helpers;
using CaseManagement.Pn.Infrastructure.Models;
using eFormApi.BasePn.Consts;
using eFormApi.BasePn.Infrastructure;
using eFormApi.BasePn.Infrastructure.Models.API;
using NLog;

namespace CaseManagement.Pn.Controllers
{
    [Authorize]
    public class CaseManagementSettingsController : ApiController
    {
        private readonly Logger _logger;
        private readonly CaseManagementPnDbContext _dbContext;
        private readonly EFormCoreHelper _coreHelper = new EFormCoreHelper();

        public CaseManagementSettingsController()
        {
                _dbContext = CaseManagementPnDbContext.Create();
                _logger = LogManager.GetCurrentClassLogger();
        }
        
        [HttpGet]
        [Authorize(Roles = EformRoles.Admin)]
        [Route("api/case-management-pn/settings")]
        public OperationDataResult<CaseManagementPnSettingsModel> GetSettings()
        {
            try
            {
                var result = new CaseManagementPnSettingsModel();
                var customerSettings = _dbContext.CaseManagementSettings.FirstOrDefault();
                if (customerSettings?.SelectedTemplateId != null && customerSettings?.RelatedEntityGroupId != null)
                {
                    result.SelectedTemplateId = (int) customerSettings.SelectedTemplateId;
                    result.SelectedTemplateName =customerSettings.SelectedTemplateName;
                    result.RelatedEntityGroupId = customerSettings.RelatedEntityGroupId;
                    var core = _coreHelper.GetCore();
                    var entityGroup = core.EntityGroupRead(customerSettings.RelatedEntityGroupId.ToString());
                    if (entityGroup == null)
                    {
                        return new OperationDataResult<CaseManagementPnSettingsModel>(false, "Entity group not found");
                    }

                    result.RelatedEntityGroupName = entityGroup.Name;

                }
                else
                {
                    result.RelatedEntityGroupId = null;
                    result.SelectedTemplateId = null;
                }
                return new OperationDataResult<CaseManagementPnSettingsModel>(true, result);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.Error(e);
                return new OperationDataResult<CaseManagementPnSettingsModel>(false,
                    CustomersPnLocaleHelper.GetString("ErrorWhileObtainingCaseManagementSettings"));
            }
        }

        [HttpPost]
        [Authorize(Roles = EformRoles.Admin)]
        [Route("api/case-management-pn/settings")]
        public OperationResult UpdateSettings(CaseManagementPnSettingsModel caseManagementSettingsModel)
        {
            try
            {
                var caseManagementSettings = _dbContext.CaseManagementSettings.FirstOrDefault();
                if (caseManagementSettings == null)
                {
                    caseManagementSettings = new CaseManagementSetting()
                    {
                        SelectedTemplateId = caseManagementSettingsModel.SelectedTemplateId,
                        RelatedEntityGroupId = caseManagementSettingsModel.RelatedEntityGroupId
                    };
                    _dbContext.CaseManagementSettings.Add(caseManagementSettings);
                }
                else
                {
                    caseManagementSettings.SelectedTemplateId = caseManagementSettingsModel.SelectedTemplateId;
                    caseManagementSettings.RelatedEntityGroupId = caseManagementSettingsModel.RelatedEntityGroupId;
                }

                if (caseManagementSettingsModel.SelectedTemplateId != null && caseManagementSettingsModel.RelatedEntityGroupId != null)
                {
                    var core = _coreHelper.GetCore();
                    var template = core.TemplateRead((int) caseManagementSettingsModel.SelectedTemplateId);
                    caseManagementSettings.SelectedTemplateName = template.Label;
                }
                _dbContext.SaveChanges();
                return new OperationResult(true,
                    CustomersPnLocaleHelper.GetString("SettingsHasBeenUpdatedSuccessfully"));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.Error(e);
                return new OperationResult(false,
                    CustomersPnLocaleHelper.GetString("ErrorWhileUpdatingCaseManagementSettings"));
            }
        }
    }
}