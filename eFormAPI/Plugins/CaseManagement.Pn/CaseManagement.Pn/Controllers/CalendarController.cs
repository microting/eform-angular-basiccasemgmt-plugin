using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Helpers;
using CaseManagement.Pn.Infrastructure.Models.Calendar;
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
        [Route("api/case-management-pn/calendar")]
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
                    .Select(x=> (int) x.Key)
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
                            var item = new CalendarEventModel
                            {
                                Color = calendarUser.Color,
                                Title = "111",
                                Start = DateTime.UtcNow,
                                End = DateTime.UtcNow,
                            };
                            var meta = new CalendarEventMeta
                            {
                                CaseId = caseItem.Id.ToString(),
                                CalendarUserName = calendarUser.NameInCalendar
                            };
                            item.Meta.Add(meta);
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
                    CustomersPnLocaleHelper.GetString("ErrorObtainingCustomersInfo"));
            }
        }
    }
}