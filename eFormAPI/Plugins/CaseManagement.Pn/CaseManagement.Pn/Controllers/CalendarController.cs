using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Helpers;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
using CaseManagement.Pn.Infrastructure.Data.Entities;
using eFormApi.BasePn.Infrastructure;
using eFormApi.BasePn.Infrastructure.Models.API;
using NLog;

namespace CaseManagement.Pn.Controllers
{
    [Authorize]
    public class CalendarController : ApiController
    {
        private readonly Logger _logger;
        private readonly CaseManagementPnDbContext _dbContext;
        private readonly EFormCoreHelper _coreHelper = new EFormCoreHelper();

        public CalendarController()
        {
            _dbContext = CaseManagementPnDbContext.Create();
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpPost]
        [Route("api/case-management-pn/calendar/events")]
        public OperationDataResult<List<CalendarEventModel>> GetCalendarEvents(CalendarEventsRequestModel requestModel)
        {
            try
            {
                List<CalendarEventModel> calendarEventModels = new List<CalendarEventModel>();
                eFormCore.Core core = _coreHelper.GetCore();
                List<eFormShared.Case> cases = core.CaseReadAll(requestModel.TemplateId, requestModel.StartDate, requestModel.EndDate);
                List<int> casesSiteIds = cases.Where(x => x.SiteId != null)
                    .Select(x => x.SiteId)
                    .GroupBy(x => x)
                    .Select(x => (int)x.Key)
                    .ToList();


                List<CalendarUser> calendarUsers = _dbContext.CalendarUsers
                    .Where(x => x.IsVisibleInCalendar)
                    .ToList();

                foreach (eFormShared.Case caseItem in cases)
                {
                    if (caseItem.SiteId != null)
                    {
                        int? siteId = caseItem.SiteId;

                        // We change this, because the reporting user is not the same as the one that is going to finish the task.
                        // This should be a dynamic change based on settings in a later version.
                        // CalendarUser calendarUser = calendarUsers.FirstOrDefault(x => x.SiteId == siteId);
                        CalendarUser calendarUser = calendarUsers.FirstOrDefault(x => x.NameInCalendar == caseItem.FieldValue3);
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
                _logger.Error(e);
                return new OperationDataResult<List<CalendarEventModel>>(false,
                    CustomersPnLocaleHelper.GetString("ErrorObtainingCalendarInfo"));
            }
        }
    }
}