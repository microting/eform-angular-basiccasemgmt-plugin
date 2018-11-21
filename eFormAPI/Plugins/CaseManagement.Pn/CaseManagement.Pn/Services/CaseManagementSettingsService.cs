using System;
using System.Diagnostics;
using System.Linq;
using CaseManagement.Pn.Abstractions;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Data.Entities;
using CaseManagement.Pn.Infrastructure.Models;
using eFormCore;
using eFormData;
using Microsoft.Extensions.Logging;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Services
{
    public class CaseManagementSettingsService : ICaseManagementSettingsService
    {
        private readonly ILogger<CaseManagementSettingsService> _logger;
        private readonly ICaseManagementLocalizationService _caseManagementLocalizationService;
        private readonly CaseManagementPnDbAnySql _dbContext;
        private readonly IEFormCoreService _coreHelper;

        public CaseManagementSettingsService(ILogger<CaseManagementSettingsService> logger, 
            CaseManagementPnDbAnySql dbContext, 
            IEFormCoreService coreHelper, 
            ICaseManagementLocalizationService caseManagementLocalizationService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _coreHelper = coreHelper;
            _caseManagementLocalizationService = caseManagementLocalizationService;
        }

        public OperationDataResult<CaseManagementPnSettingsModel> GetSettings()
        {
            try
            {
                CaseManagementPnSettingsModel result = new CaseManagementPnSettingsModel();
                CaseManagementSetting customerSettings = _dbContext.CaseManagementSettings.FirstOrDefault();
                if (customerSettings?.SelectedTemplateId != null && customerSettings?.RelatedEntityGroupId != null)
                {
                    result.SelectedTemplateId = (int) customerSettings.SelectedTemplateId;
                    result.SelectedTemplateName = customerSettings.SelectedTemplateName;
                    result.RelatedEntityGroupId = customerSettings.RelatedEntityGroupId;
                    Core core = _coreHelper.GetCore();
                    EntityGroup entityGroup = core.EntityGroupRead(customerSettings.RelatedEntityGroupId.ToString());
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
                _logger.LogError(e.Message);
                return new OperationDataResult<CaseManagementPnSettingsModel>(false,
                    _caseManagementLocalizationService.GetString("ErrorWhileObtainingCaseManagementSettings"));
            }
        }

        public OperationResult UpdateSettings(CaseManagementPnSettingsModel caseManagementSettingsModel)
        {
            try
            {
                if (caseManagementSettingsModel.RelatedEntityGroupId == 0 || caseManagementSettingsModel.SelectedTemplateId == 0)
                {
                    return new OperationResult(true);
                }
                CaseManagementSetting caseManagementSettings = _dbContext.CaseManagementSettings.FirstOrDefault();
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

                if (caseManagementSettingsModel.SelectedTemplateId != null &&
                    caseManagementSettingsModel.RelatedEntityGroupId != null)
                {
                    Core core = _coreHelper.GetCore();
                    MainElement template = core.TemplateRead((int) caseManagementSettingsModel.SelectedTemplateId);
                    caseManagementSettings.SelectedTemplateName = template.Label;
                }

                _dbContext.SaveChanges();
                return new OperationResult(true,
                    _caseManagementLocalizationService.GetString("SettingsHasBeenUpdatedSuccessfully"));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.LogError(e.Message);
                return new OperationResult(false,
                    _caseManagementLocalizationService.GetString("ErrorWhileUpdatingCaseManagementSettings"));
            }
        }
    }
}