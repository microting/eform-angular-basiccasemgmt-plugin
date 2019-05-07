using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CaseManagement.Pn.Abstractions;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Data.Entities;
using CaseManagement.Pn.Infrastructure.Models;
using eFormCore;
using eFormData;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using Microting.eFormApi.BasePn.Infrastructure.Helpers.PluginDbOptions;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Services
{
    public class CaseManagementSettingsService : ICaseManagementSettingsService
    {
        private readonly ILogger<CaseManagementSettingsService> _logger;
        private readonly ICaseManagementLocalizationService _caseManagementLocalizationService;
        private readonly CaseManagementPnDbContext _dbContext;
        private readonly IEFormCoreService _coreHelper;
        private readonly IPluginDbOptions<CaseManagementBaseSettings> _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CaseManagementSettingsService(ILogger<CaseManagementSettingsService> logger, 
            CaseManagementPnDbContext dbContext, 
            IEFormCoreService coreHelper, 
            IPluginDbOptions<CaseManagementBaseSettings> options,
            ICaseManagementLocalizationService caseManagementLocalizationService,    
            IHttpContextAccessor httpContextAccessor)

        {
            _logger = logger;
            _dbContext = dbContext;
            _coreHelper = coreHelper;
            _caseManagementLocalizationService = caseManagementLocalizationService;
            _options = options;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationDataResult<CaseManagementBaseSettings>> GetSettings()
        {
            try
            {
                var option = _options.Value;
                if (option.SdkConnectionString == "...")
                {
                    string connectionString = _dbContext.Database.GetDbConnection().ConnectionString;

                    string dbNameSection = Regex.Match(connectionString, @"(Database=(...)_eform-angular-\w*-plugin;)").Groups[0].Value;
                    string dbPrefix = Regex.Match(connectionString, @"Database=(\d*)_").Groups[1].Value;
                    string sdk = $"Database={dbPrefix}_SDK;";
                    connectionString = connectionString.Replace(dbNameSection, sdk);
                    await _options.UpdateDb(settings => { settings.SdkConnectionString = connectionString;}, _dbContext, UserId);

                }

//                CaseManagementPnSettingsModel result = new CaseManagementPnSettingsModel();
//                CaseManagementSetting customerSettings = _dbContext.CaseManagementSettings.FirstOrDefault();
//                if (customerSettings?.SelectedTemplateId != null && customerSettings?.RelatedEntityGroupId != null)
//                {
//                    result.SelectedTemplateId = (int) customerSettings.SelectedTemplateId;
//                    result.SelectedTemplateName = customerSettings.SelectedTemplateName;
//                    result.RelatedEntityGroupId = customerSettings.RelatedEntityGroupId;
//                    Core core = _coreHelper.GetCore();
//                    EntityGroup entityGroup = core.EntityGroupRead(customerSettings.RelatedEntityGroupId.ToString());
//                    if (entityGroup == null)
//                    {
//                        return new OperationDataResult<CaseManagementPnSettingsModel>(false, "Entity group not found");
//                    }
//
//                    result.RelatedEntityGroupName = entityGroup.Name;
//                }
//                else
//                {
//                    result.RelatedEntityGroupId = null;
//                    result.SelectedTemplateId = null;
//                }

                return new OperationDataResult<CaseManagementBaseSettings>(true, option);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.LogError(e.Message);
                return new OperationDataResult<CaseManagementBaseSettings>(false,
                    _caseManagementLocalizationService.GetString("ErrorWhileObtainingCaseManagementSettings"));
            }
        }

        public async Task<OperationResult> UpdateSettings(CaseManagementBaseSettings caseManagementSettingsModel)
        {
            try
            {
                if (caseManagementSettingsModel.RelatedEntityGroupId == 0 || caseManagementSettingsModel.SelectedTemplateId == 0)
                {
                    return new OperationResult(true);
                }

                await _options.UpdateDb(settings =>
                {
                    settings.LogLevel = caseManagementSettingsModel.LogLevel;
                    settings.LogLimit = caseManagementSettingsModel.LogLimit;
                    settings.MaxParallelism = caseManagementSettingsModel.MaxParallelism;
                    settings.NumberOfWorkers = caseManagementSettingsModel.NumberOfWorkers;
                    settings.SelectedTemplateId = caseManagementSettingsModel.SelectedTemplateId;
                    settings.SdkConnectionString = caseManagementSettingsModel.SdkConnectionString;
                    settings.RelatedEntityGroupId = caseManagementSettingsModel.RelatedEntityGroupId;
                }, _dbContext, UserId);
                //                PluginConfigurationValue caseManagementSettings = _dbContext.PluginConfigurationValues.FirstOrDefault();
//                if (caseManagementSettings == null)
//                {
//                    caseManagementSettings = new PluginConfigurationValue()
//                    {
//                        Name = caseManagementSettingsModel.SelectedTemplateId,
//                        Value = caseManagementSettingsModel.RelatedEntityGroupId
//                    };
//                    _dbContext.CaseManagementSettings.Add(caseManagementSettings);
//                }
//                else
//                {
//                    caseManagementSettings.SelectedTemplateId = caseManagementSettingsModel.SelectedTemplateId;
//                    caseManagementSettings.RelatedEntityGroupId = caseManagementSettingsModel.RelatedEntityGroupId;
//                }
//
//                if (caseManagementSettingsModel.SelectedTemplateId != null &&
//                    caseManagementSettingsModel.RelatedEntityGroupId != null)
//                {
//                    Core core = _coreHelper.GetCore();
//                    MainElement template = core.TemplateRead((int) caseManagementSettingsModel.SelectedTemplateId);
//                    caseManagementSettings.SelectedTemplateName = template.Label;
//                }

//                _dbContext.SaveChanges();
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
        public int UserId
        {
            get
            {
                var value = _httpContextAccessor?.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return value == null ? 0 : int.Parse(value);
            }
        }
    }
}