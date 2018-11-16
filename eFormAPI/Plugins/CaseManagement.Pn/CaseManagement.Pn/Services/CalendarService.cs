using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using CaseManagement.Pn.Abstractions;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
using Microsoft.Extensions.Logging;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace CaseManagement.Pn.Services
{
   public class CalendarService : ICalendarService
    {
        private readonly ILogger<CalendarService> _logger;
        private readonly CaseManagementPnDbAnySql _dbContext;
        private readonly ICaseManagementLocalizationService _caseManagementLocalizationService;
        private readonly IEFormCoreService _coreHelper;

        public CalendarService(ILogger<CalendarService> logger, 
            CaseManagementPnDbAnySql dbContext, 
            IEFormCoreService coreHelper, 
            ICaseManagementLocalizationService caseManagementLocalizationService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _coreHelper = coreHelper;
            _caseManagementLocalizationService = caseManagementLocalizationService;
        }

        public OperationDataResult<List<CalendarEventModel>> GetCalendarEvents(CalendarEventsRequestModel requestModel)
        {
            try
            {
                var calendarEventModels = new List<CalendarEventModel>();
                var core = _coreHelper.GetCore();
                var cases = core.CaseReadAll(requestModel.TemplateId, requestModel.StartDate, requestModel.EndDate);
                var casesSiteIds = cases.Where(x => x.SiteId != null)
                    .Select(x => x.SiteId)
                    .GroupBy(x => x)
                    .Select(x => (int) x.Key)
                    .ToList();

                var calendarUsers = _dbContext.CalendarUsers
                    .Where(x => casesSiteIds.Contains(x.SiteId) && x.IsVisibleInCalendar)
                    .ToList();

                foreach (var caseItem in cases)
                {
                    if (caseItem.SiteId != null)
                    {
                        var siteId = caseItem.SiteId;
                        var calendarUser = calendarUsers.FirstOrDefault(x => x.SiteId == siteId);
                        if (calendarUser != null)
                        {
                            var dateParseResult = DateTime.TryParseExact(caseItem.FieldValue1, 
                                "yyyy-MM-dd", 
                                null,
                                DateTimeStyles.None, 
                                out DateTime date);
                            var item = new CalendarEventModel
                            {
                                Color = calendarUser.Color,
                                Title = caseItem.FieldValue2
                            };
                            if (dateParseResult)
                            {
                                item.Start = date;
                                item.End = date;
                            }
                            item.Meta = new CalendarEventMeta
                            {
                                CaseId = caseItem.Id.ToString(),
                                CalendarUserName = calendarUser.NameInCalendar
                            };
                            // Add item
                            calendarEventModels.Add(item);
                        }
                    }
                }

                return new OperationDataResult<List<CalendarEventModel>>(true, calendarEventModels);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.LogError(e.Message);
                return new OperationDataResult<List<CalendarEventModel>>(false,
                    _caseManagementLocalizationService.GetString("ErrorObtainingCalendarInfo"));
            }
        }
    }
}
